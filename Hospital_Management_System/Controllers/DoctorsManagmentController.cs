using demo.Models;
using Hospital_Management_System.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                Country ="Egypt",
                Shift="Day"
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



        //frist look with edit

        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    ViewBag.dept = _context.Departments.ToList();


        //    var data =
        //  from s in _context.Staffs
        //  join d in _context.Doctors
        //  on s.ID equals id
        //  join dept in _context.Departments
        //  on s.dept_id equals dept.Id
        //  select new StaffDetailsVM
        //  {
        //      Id = id,
        //      FullName = s.FullName,
        //      Gender = s.Gender,
        //      BDate = s.BDate,
        //      Phone = s.Phone,
        //      Role = s.Role,
        //      Email = s.Email,
        //      Specialization = d.Specialization,
        //      DepartmentName = dept.Name,
        //      Qualifications = s.Qualifications,
        //      Governorate = s.Governorate,
        //      City = s.City,
        //      Password = s.Password
        //  };



        //    return View( data.ToList().FirstOrDefault());
        //}

        //[HttpPost]
        //public ActionResult SaveEdit(int id,StaffDetailsVM NewModel) 
        //{

        //    if(!ModelState.IsValid)
        //    {
        //        return View("Index");
        //    }


        //    try
        //    {
        //        var doctor = _context.Doctors
        //            .Include(d => d.Staff)
        //            .FirstOrDefault(d => d.Id == id);
        //        var department = _context.Departments.FirstOrDefault(d => d.Id == id);
        //        doctor.Staff.FullName = NewModel.FullName;
        //        doctor.Staff.Gender = NewModel.Gender;
        //        doctor.Staff.BDate = NewModel.BDate;
        //        doctor.Staff.Phone = NewModel.Phone;
        //        doctor.Staff.Role = NewModel.Role;
        //        doctor.Staff.Email = NewModel.Email;
        //        doctor.Specialization = NewModel.Specialization;
        //        doctor.Staff.Qualifications = NewModel.Qualifications;
        //        doctor.Staff.Governorate = NewModel.Governorate;
        //        doctor.Staff.City = NewModel.City;
        //        doctor.Staff.Password = NewModel.Password;
        //        department.Name = NewModel.DepartmentName;
        //        _context.SaveChanges();
        //    } catch (Exception ex)
        //    {
        //        ViewBag.Error=ex.Message;
        //    }


        //    return RedirectToAction("DoctorDetails", new { DoctorId = id });


        //}


    }
}