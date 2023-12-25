using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
	{
		HMSEntites context = new HMSEntites();
		public ActionResult MngDoctors()
		{

			return View("MngDoctors");
		}
		public ActionResult MngNurses()
		{



			List<StaffViewModel> Nurses = new();
			Nurses = (
			 from s in context.Staffs
			 join n in context.Nurses on s.ID equals n.Id
			 select new StaffViewModel
			 {
				 FullName = s.FullName,
				 Phone = s.Phone,
				 Email = s.Email,
				 Salary = s.Salary,
				 Specialization = n.Specialization
			 }
		 ).ToList();
			return View("MngNurses", Nurses);
		}
		public ActionResult MngPatients()
		{
			List<DoctorPatient> patients = new();
			patients = (
		   from p in context.Patients
		   join m in context.MedicalRecords on p.ID equals m.Patient_id
		   join d in context.Doctors on m.Doctor_id equals d.Id
		   join s in context.Staffs on d.Id equals s.ID
		   select new DoctorPatient
		   {
			   FullName = p.FullName,
			   Gender = p.Gender,
			   BDate = p.BDate,
			   Phone = p.Phone,
			   Email = p.Email,
			   FullNameDoctor = s.FullName
		   }
	   ).ToList();
			return View("MngPatients", patients);
		}
		public IActionResult Admin()
		{

			var adminData = (from s in context.Staffs
							 join a in context.Administrators on s.ID equals a.Id
							 select new
							 {
								 FullName = s.FullName,
								 Gender = s.Gender,
								 AdministrationType = a.Specialty
							 }).ToList();



			List<StaffViewModel> staff = new List<StaffViewModel>();
			foreach (var item in adminData)
			{
				StaffViewModel listItem = new Hospital_Management_System.ViewModel.StaffViewModel();
				listItem.FullName = item.FullName;

				listItem.Gender = item.Gender;
				listItem.Admin.Specialty = item.AdministrationType;


				//add your remaining fields
				staff.Add(listItem);
			}



			return View("Admin", staff);
		}
	}
}
