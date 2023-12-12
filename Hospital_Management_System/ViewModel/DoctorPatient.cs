using demo.Models;
using Hospital_Management_System.Models;

namespace Hospital_Management_System.ViewModel
{
	public class DoctorPatient : Person
	{
		public string FullNameDoctor { get; set; }
		
		public Doctor Doctor { get; set; }
		public Patient Patient { get; set; }
		public DateOnly Date { get; set; }
		public DateTime time { get; set; }
		public string Status { get; set; }

		public MedicalRecord MedicalRecord { get; set; }
		
	}
}
