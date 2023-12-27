using demo.Models;
using Hospital_Management_System.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management_System.Controllers
{
    [Authorize]
    public class DoctorsManagmentController : Controller
    {
        HMSEntites _context = new HMSEntites();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInMAnager;

        public DoctorsManagmentController
            (UserManager<ApplicationUser> _UserManager,
            SignInManager<ApplicationUser> _SignInMAnager)
        {
            userManager = _UserManager;
            signInMAnager = _SignInMAnager;
        }

        public ActionResult Index()
        {
            //full name and department --details->(update-delete)
            var data =
       from s in _context.Staffs
       join dept in _context.Departments on s.dept_id equals dept.Id
       join d in _context.Doctors on s.ID equals d.Id
       select new StaffVM
       {
           Name = s.FullName,
           DeptName = dept.Name,
           Id = d.Id,
       };
            List<StaffVM> result = data.ToList();

            return View("Index", result);
        }
        public ActionResult DoctorDetails(int id)
        {

            var data =
                  from s in _context.Staffs
                  join d in _context.Doctors
                  on s.ID equals id
                  join dept in _context.Departments
                  on s.dept_id equals dept.Id
                  where d.Id == id
                  select new StaffDetailsVM
                  {
                      Id = id,
                      FullName = s.FullName,
                      Gender = s.Gender,
                      BDate = s.BDate,
                      Phone = s.Phone,
                      Role = s.Role,
                      Email = s.Email,
                      Specialization = d.Specialization,
                      DepartmentName = dept.Name,
                      Qualifications = s.Qualifications,
                      Governorate = s.Governorate,
                      City = s.City
                  };
                    return View("DoctorDetails", data.ToList().FirstOrDefault());
        }
        public IActionResult AddDoctor()
        {
            ViewBag.departments= _context.Departments.ToList();

			return View("AddDoctor"); 

        }

        //Save doctor in Database 
        [HttpPost]
        public IActionResult SaveDoctor(AddDoctorVM model)
        {

			if (!ModelState.IsValid)
            {
                return RedirectToAction("AddDoctor");
            }

            var staff = new Staff
            {
                FullName = model.FullName,
                Gender = model.Gender,
                BDate = model.BDate,
                Phone = model.Phone,
                Email = model.Email,
                Password = model.Password,
                Salary = model.Salary,
                Role = model.Role,
                Qualifications = model.Qualifications,
                dept_id = model.DeptId,
                hospital_id = 1,
                Governorate = model.Governorate,
                City = model.City,
                Country = model.Country,
                Shift= model.Shift
            };

            var doctor = new Doctor
            {
                Specialization = model.Specialization,
                Staff = staff
            };



            try
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }


        public ActionResult DeleteDoctor(int id)
        {

                try
                {
                    var doctorToDelete = _context.Doctors.FirstOrDefault(d => d.Id == id);

                if (doctorToDelete == null)
                    {
                        ViewBag.ErrorMessage = "Doctor not found.";
                        return View("Error");
                    }

                    _context.Doctors.Remove(doctorToDelete);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = ex.Message;
                    return View("Error");
                }
            
        }



		public IActionResult EditDoctor(int id)
		{
			var doctor = _context.Doctors
								.Include(d => d.Staff)
								.Include(d => d.Staff.Department)
								.FirstOrDefault(d => d.Id == id);

			if (doctor == null)
			{
				ViewBag.ErrorMessage = "Doctor not found.";
				return View("Error");
			}

			var editDoctorVM = new EditDoctorVM
			{
				Id = doctor.Id,
				FullName = doctor.Staff.FullName,
				Gender = doctor.Staff.Gender,
				BDate = doctor.Staff.BDate,
				Phone = doctor.Staff.Phone,
				Email = doctor.Staff.Email,
				Password = doctor.Staff.Password,
				Salary = doctor.Staff.Salary,
				Role = doctor.Staff.Role,
				Qualifications = doctor.Staff.Qualifications,
				DeptId = doctor.Staff.dept_id,
				Governorate = doctor.Staff.Governorate,
				City = doctor.Staff.City,
				Specialization = doctor.Specialization,
				Shift = doctor.Staff.Shift,
				Country = doctor.Staff.Country
			};

			ViewBag.Departments = _context.Departments.ToList();

			return View("EditDoctor", editDoctorVM);
		}

		[HttpPost]
		public IActionResult EditDoctor(int id, EditDoctorVM editedDoctor)
		{

			if (!ModelState.IsValid)
			{
				return RedirectToAction("AddDoctor");
			}

			var existingDoctor = _context.Doctors
										.Include(d => d.Staff)
										.FirstOrDefault(d => d.Id == id);

			if (existingDoctor == null)
			{
				ViewBag.ErrorMessage = "Doctor not found.";
				return View("Error");
			}

			existingDoctor.Staff.FullName = editedDoctor.FullName;
			existingDoctor.Staff.Gender = editedDoctor.Gender;
			existingDoctor.Staff.BDate = editedDoctor.BDate;
			existingDoctor.Staff.Phone = editedDoctor.Phone;
			existingDoctor.Staff.Email = editedDoctor.Email;
			existingDoctor.Staff.Password = editedDoctor.Password;
			existingDoctor.Staff.Salary = editedDoctor.Salary;
			existingDoctor.Staff.Role = editedDoctor.Role;
			existingDoctor.Staff.Qualifications = editedDoctor.Qualifications;
			existingDoctor.Staff.dept_id = editedDoctor.DeptId;
			existingDoctor.Staff.Governorate = editedDoctor.Governorate;
			existingDoctor.Staff.City = editedDoctor.City;
			existingDoctor.Specialization = editedDoctor.Specialization;
			existingDoctor.Staff.Shift = editedDoctor.Shift;
			existingDoctor.Staff.Country = editedDoctor.Country;

			try
			{
				_context.SaveChanges();
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
				ViewBag.Departments = _context.Departments.ToList();
				return View("EditDoctor", editedDoctor);
			}

			return RedirectToAction("Index");
		}



	}
}