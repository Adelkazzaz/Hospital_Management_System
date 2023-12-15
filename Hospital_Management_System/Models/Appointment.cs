using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class Appointment
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Status { get; set; }



		[ForeignKey("Patient")]
		public int Patient_id { get; set; }
		public virtual Patient Patient { get; set; }

		[ForeignKey("Doctor")]
		public int Doctor_id { get; set; }
		public virtual Doctor Doctor { get; set; }

	}
}
