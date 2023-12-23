using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace demo.Models
{
	public class Appointment
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter a date")]
		public DateTime Date { get; set; }
		public string Status { get; set; }



		[ForeignKey("Patient")]
		[Required(ErrorMessage = "Patient ID is Requreid")]
		public int Patient_id { get; set; }
        public virtual Patient Patient { get; set; }

		[ForeignKey("Doctor")]
		public int Doctor_id { get; set; }
		public virtual Doctor Doctor { get; set; }

	}
}
