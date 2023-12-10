using demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        HMSEntites context = new HMSEntites();
        public IActionResult Index()
        { 
           
           List<demo.Models.Department> departments1= context.Departments.ToList();
            ViewData["Dept"] = departments1;

            List<Staff> doctorsInDepartment = context.Staffs
               .Where(s => s.JobTitle == "Doctor" && s.ID == s.dept_id)
               .ToList();


            return View("doctors",doctorsInDepartment);
        }
    }
}
