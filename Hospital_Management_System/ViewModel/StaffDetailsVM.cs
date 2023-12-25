using demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.ViewModel
{
    public class StaffDetailsVM :AddDoctorVM
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DeptId { get; set; }
        public IEnumerable<Department> Department { get; set; }
    }
}
