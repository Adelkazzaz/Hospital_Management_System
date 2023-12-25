
using Hospital_Management_System.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace demo.Models
{
	public class Patient : Person
	{
		public string Blood_Type { get; set; }

		[ForeignKey("Nurse")]
		public int Nurseid { get; set; }
		public virtual Nurse Nurse { get; set; }

		public virtual ICollection<MedicalRecord> MedicalRecord { get; set; }
		public virtual ICollection<Appointment> Appointments { get; set; }
	}
}
