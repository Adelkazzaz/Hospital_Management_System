using demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        HMSEntites context = new HMSEntites();
        public IActionResult Index()
        {
            List<Department> departments = context.Departments.ToList();
            ViewData["Dept"] = departments;

            List<Staff> doctorsInDepartment = context.Staffs
               .Where(s => s.JobTitle == "Doctor" && s.ID == s.dept_id)
               .ToList();


            return View("doctors",doctorsInDepartment);
        }
    }
}
