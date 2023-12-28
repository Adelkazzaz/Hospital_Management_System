using demo.Models;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.ViewModel
{
    public class StaffDetailsVM
    {
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Gender { get; set; }
		public DateOnly BDate { get; set; }
		public string Phone { get; set; }
		public string UserName { get; set; }
		public string Governorate { get; set; }
		public string City { get; set; }
		public int Salary { get; set; }
		public string Role { get; set; }
		public string Specialization { get; set; }
		public string Qualifications { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DeptId { get; set; }
        public IEnumerable<Department> Department { get; set; }
    }
}
