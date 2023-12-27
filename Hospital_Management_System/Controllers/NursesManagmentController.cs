using demo.Models;
using Hospital_Management_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management_System.Controllers
{
    [Authorize]
    public class NursesManagmentController : Controller
	{
		HMSEntites _context = new HMSEntites();
		public ActionResult Index()
		{
			//full name and department --details->(update-delete)
			var data =
	   from s in _context.Staffs
	   join dept in _context.Departments 
	   on s.dept_id equals dept.Id
	   join n in _context.Nurses 
	   on s.ID equals n.Id
	   select new StaffVM
	   {
		   Name = s.FullName,
		   DeptName = dept.Name,
		   Id = n.Id,
	   };
			List<StaffVM> result = data.ToList();

			return View("Index", result);
		}
		public ActionResult NurseDetails(int id)
		{

			var data =
		  from s in _context.Staffs
		  join n in _context.Nurses
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
			  Specialization = n.Specialization,
			  DepartmentName = dept.Name,
			  Qualifications = s.Qualifications,
			  Governorate = s.Governorate,
			  City = s.City
		  };



			return View("NurseDetails", data.ToList().FirstOrDefault());
		}
	}
}
