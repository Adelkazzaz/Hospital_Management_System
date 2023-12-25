using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class MedicalRecord
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Please Enter a Date")]
		public DateOnly StartDate { get; set; }
        [Required(ErrorMessage = "Please Enter a Treatment Doctor!")]
        public string Treatment { get; set; }
        [Required(ErrorMessage = "Please Enter a Diagnosis Doctor!")]
        public string Diagnosis { get; set; }

		[ForeignKey("Patient")]

        [Required(ErrorMessage = "Please Enter a Pateint ID")]

        public int Patient_id { get; set; }
		public virtual Patient Patient { get; set; }


		[ForeignKey("Doctor")]
		public int Doctor_id { get; set; }
		public virtual Doctor Doctor { get; set; }

	}
}
