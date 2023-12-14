using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Hospital_Management_System.Controllers
{
	public class DoctorController : Controller
	{
		HMSEntites context = new HMSEntites();
		public IActionResult DashBoard()
		{
			HMSEntites context = new HMSEntites();

			List<DoctorPatient> Detail = new();
			Detail = (
		   from p in context.Patients
		   join a in context.Appointments on p.ID equals a.Patient_id
		   select new DoctorPatient
		   {
			   FullName = p.FullName,
			   time = a.Date,
			   Status = a.Status
		   }
	   ).ToList();
			return View("DashBoard", Detail);
		}
		public IActionResult CreateAppointment()
		{
			return View();
		}
		[HttpPost]
		public IActionResult CreateAppointment(Appointment appointment)
		{
			context.Appointments.Add(appointment);
			context.SaveChanges();
			return RedirectToAction("DisplayAppointment");
		}
		public IActionResult DisplayAppointment()
		{


			List<Appointment> appointments = context.Appointments.ToList();

			return View("DisplayAppointment", appointments);
		}

		public IActionResult AddMedicalRecord()
		{
			return View();
		}
		[HttpPost]
		public IActionResult AddMedicalRecord(MedicalRecord MRecord)
		{
			context.MedicalRecords.Add(MRecord);
			context.SaveChanges();
			return View();
		}
		public IActionResult DisplayMedicalRecords(int id)
		{
			List<MedicalRecord> MedRec = context.MedicalRecords.Where(mr => mr.Patient_id == id).ToList();

			return View("ViewRecords", MedRec);
		}
	}
}
