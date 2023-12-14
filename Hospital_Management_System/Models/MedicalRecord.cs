using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class MedicalRecord
	{
		public int Id { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public string Treatment { get; set; }
		public string Diagnosis { get; set; }

		[ForeignKey("Patient")]
		public int Patient_id { get; set; }
		public virtual Patient Patient { get; set; }


		[ForeignKey("Doctor")]
		public int Doctor_id { get; set; }
		public virtual Doctor Doctor { get; set; }

	}
}
