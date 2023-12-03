using System.Globalization;
using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace demo.Models
{
    public class Staff
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateOnly BDate { get; set; }
        public int Salary { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string jobTitle { get; set; }
        public string Password { get; set; }

        [ForeignKey("Department")]
        public int dept_id { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("Hospital")]
        public int hospital_id { get; set; }
        public virtual Hospital Hospital { get; set; }

        public virtual List<Doctor> Doctors { get; set; }
        public virtual List<Nurce> Nurces { get; set; }
        public virtual List<Administrator> Admins { get; set; }


    }
}
