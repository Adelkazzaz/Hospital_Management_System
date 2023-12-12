
using Hospital_Management_System.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace demo.Models
{
	public class Patient : Person
	{

		[ForeignKey("Nurce")]
		public int Nurce_id { get; set; }
		public virtual Nurse Nurse { get; set; }

		public virtual ICollection<MedicalRecord> MedicalRecord { get; set; }
		public virtual ICollection<Appointment> Appointments { get; set; }
	}
}
