using System.Globalization;
using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Hospital_Management_System.Models;
using System.Collections;

namespace demo.Models
{
	public class Staff : Person
	{

		public int Salary { get; set; }
		public string Role { get; set; }
		public int No_Of_Hour { get; set; }
		public string Shift { get; set; }

		[ForeignKey("Department")]
		public int dept_id { get; set; }
		public virtual Department Department { get; set; }

		[ForeignKey("Hospital")]
		public int hospital_id { get; set; }
		public virtual Hospital Hospital { get; set; }

		public virtual ICollection<Doctor> Doctors { get; set; }
		public virtual ICollection<Nurse> Nurces { get; set; }
		public virtual ICollection<Administrator> Admins { get; set; }


	}
}
