using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Office { get; set; }


        public virtual List<Staff> StaffList { get; set; }
    }
}