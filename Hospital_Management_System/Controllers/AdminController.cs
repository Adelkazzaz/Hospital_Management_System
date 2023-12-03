using demo.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Hospital_Management_System.Controllers
{
    public class AdminController : Controller
    {
        HMSEntites context = new HMSEntites();
        public IActionResult Admin()
        {

            var adminData = (from s in context.Staffs
                             join a in context.Administrators on s.ID equals a.Id
                             select new
                             {
                                 FullName = s.FullName,
                                 Gender = s.Gender,
                                 AdministrationType = a.administration_type
                             }).ToList();

           

            List<StaffViewModel> staff = new List<StaffViewModel>();
            foreach (var item in adminData)
            {
                demo.Models.StaffViewModel listItem = new demo.Models.StaffViewModel();
                listItem.FullName = item.FullName;

                listItem.Gender = item.Gender;
                listItem.administration_type = item.AdministrationType;


                //add your remaining fields
                staff.Add(listItem);
            }



            return View("Admin",staff);
        }
    }
}
