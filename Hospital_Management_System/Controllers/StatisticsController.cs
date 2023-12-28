using Microsoft.AspNetCore.Mvc;
using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management_System.Controllers
{
	public class StatisticsController : Controller
	{
		HMSEntites context = new HMSEntites();
		public IActionResult Index()
		{
			List<StatisticsHospitalVM> statistics = new();

			statistics.Add(GetGenderRisk("Common Cold"));
			statistics.Add(GetDoctorAppointmentCount(91, 12));
			statistics.Add(GetGovernorateDiseaseCount("Egypt"));
			statistics.Add(GetDeathCount());
			statistics.Add(GetAvgAgeByDiagnosis());
			statistics.Add(GetDepartmentPatientCount());
			statistics.Add(GetMostCommonDiagnosis());
			statistics.Add(GetTopTreatments());
			statistics.Add(GetAverageSalary());
			statistics.Add(BloodTypeStatistics());
			statistics.Add(GetDashboardStatistics());
			return View(statistics);
		}

		private StatisticsHospitalVM GetDoctorAppointmentCount(int doctorId, int month)
		{
			var query = from a in context.Appointments
						join s in context.Staffs on a.Doctor_id equals s.ID
						where s.ID == doctorId && a.Date.Month == month
						group a by s.FullName into doctorGroup
						select new StatisticsHospitalVM
						{
							DoctorFullName = doctorGroup.Key,
							AppointmentCount = doctorGroup.Count()
						};
            return query.FirstOrDefault();
		}
		private StatisticsHospitalVM GetAverageSalary()
		{
			double averageSalary = context.Staffs
				.Average(s => s.Salary);

			return new StatisticsHospitalVM
			{
				AverageSalary = averageSalary
			};
		}
		private StatisticsHospitalVM BloodTypeStatistics()
		{
			var topBloodType = context.Patients
				.Join(context.MedicalRecords, p => p.ID, mr => mr.Patient_id, (p, mr) => new { p, mr })
				.Where(joined => joined.mr.Diagnosis == "Migraine")
				.GroupBy(joined => joined.p.Blood_Type)
				.Select(group => new StatisticsHospitalVM
				{
					BloodType = group.Key,
					PatientCount = group.Count()
				})
				.OrderByDescending(result => result.PatientCount)
				.FirstOrDefault();

			return topBloodType;
		}
		private StatisticsHospitalVM GetGenderRisk(string diagnosis)
		{
			var query = from p in context.Patients
						join mr in context.MedicalRecords on p.ID equals mr.Patient_id
						where mr.Diagnosis == diagnosis
						group p by p.Gender into genderGroup
						orderby genderGroup.Count() descending
						select new StatisticsHospitalVM
						{
							Gender = genderGroup.Key,
							RiskCount = genderGroup.Count()
						};

			return query.FirstOrDefault();
		}

		private StatisticsHospitalVM GetGovernorateDiseaseCount(string governorate)
		{
			var query = from m in context.MedicalRecords
						join p in context.Patients on m.Patient_id equals p.ID
						where p.Governorate == governorate
						group m by new { governorate, m.Diagnosis } into diseaseGroup
						select new StatisticsHospitalVM
						{
							Governorate = diseaseGroup.Key.governorate,
							Diagnosis = diseaseGroup.Key.Diagnosis,
							DiseaseCount = diseaseGroup.Count()
						};

			return query.OrderByDescending(d => d.DiseaseCount).FirstOrDefault();
		}

		private StatisticsHospitalVM GetDeathCount()
		{
			var query = from m in context.MedicalRecords
						where m.Diagnosis.StartsWith("deceased by") && m.Diagnosis.Contains("cancer")
						select new StatisticsHospitalVM
						{
							DeathCount = m.Id
						};

			return new StatisticsHospitalVM
			{
				DeathCount = query.Count()
			};
		}

		private StatisticsHospitalVM GetAvgAgeByDiagnosis()
		{
			var query = from p in context.Patients
						join m in context.MedicalRecords on p.ID equals m.Patient_id
						group new { p, m } by m.Diagnosis into g
						select new DiagnosisAgeItem
						{
							Diagnosis = g.Key,
							AvgAge = g.Average(x => (DateTime.Now.Year - x.p.BDate.Year))
						};

			var viewModel = new StatisticsHospitalVM
			{
				AvgAgesByDiagnosis = query.ToList()
			};

			return viewModel;
		}

		private StatisticsHospitalVM GetDepartmentPatientCount()
		{
			var query = from d in context.Departments
						join s in context.Staffs on d.Id equals s.dept_id
						join m in context.MedicalRecords on s.ID equals m.Doctor_id
						join p in context.Patients on m.Patient_id equals p.ID
						group p by new { d.Id, d.Name } into g
						select new StatisticsHospitalVM
						{
							DepartmentName = g.Key.Name,
							PatientCount = g.Select(p => p.ID).Distinct().Count()
						};

			var departmentPatientCounts = query.OrderByDescending(x => x.PatientCount).ToList();

			var viewModel = new StatisticsHospitalVM
			{
				DepartmentName = departmentPatientCounts.FirstOrDefault()?.DepartmentName,
				PatientCount = departmentPatientCounts.FirstOrDefault().PatientCount
			};

			return viewModel;
		}
		private StatisticsHospitalVM GetMostCommonDiagnosis()
		{
			var query = from m in context.MedicalRecords
						join p in context.Patients on m.Patient_id equals p.ID
						group new { m, p } by new { m.Diagnosis, Month = m.StartDate.Month } into g
						orderby g.Count() descending
						select new StatisticsHospitalVM
						{
							Diagnosis = g.Key.Diagnosis,
							Month = g.Key.Month,
							DiseaseCount = g.Count()
						};

			var result = query.FirstOrDefault();

			return result;
		}
		public StatisticsHospitalVM GetTopTreatments()
		{
			var query = from m in context.MedicalRecords
						join p in context.Patients on m.Patient_id equals p.ID
						group m by m.Treatment into g
						orderby g.Count() descending
						select new TopTreatments
						{
							Treatment = g.Key,
							MedicalCount = g.Count()
						};

			var topTreatments = query.Take(10).ToList();

			var viewModel = new StatisticsHospitalVM
			{
				topTreatments = topTreatments
			};

			return viewModel;
		}
		private StatisticsHospitalVM GetDashboardStatistics()
		{
			var viewModel = new StatisticsHospitalVM
			{
				DepartmentCount = context.Departments.Count(),
				DoctorCount = context.Staffs.Count(s => s.Role == "Doctor"),
				NurseCount = context.Staffs.Count(s => s.Role == "Nurse"),
				PatientCount = context.Patients.Count()
			};

			return viewModel;
		}
	}
}
