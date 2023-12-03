using System.Globalization;
using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Hospital_Management_System.Models;

namespace demo.Models
{
    public class Staff : Person
    {
        
        public int Salary { get; set; }
        public string JobTitle { get; set; }

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
