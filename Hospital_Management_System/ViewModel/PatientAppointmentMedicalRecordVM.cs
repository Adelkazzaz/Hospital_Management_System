using demo.Models;
using System.Reflection.Metadata.Ecma335;

namespace Hospital_Management_System.ViewModel
{
	public class PatientAppointmentMedicalRecordVM
	{
        public Patient Patient  { get; set; }
		public Staff Staff { get; set; }
		public Doctor doctor { get; set; }
		public Appointment Appointment { get; set; }
		public List<MedicalRecord> MedicalRecords { get; set; }
    }
}
