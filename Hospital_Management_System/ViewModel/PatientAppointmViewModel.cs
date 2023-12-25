using demo.Models;

namespace Hospital_Management_System.ViewModel
{
    public class PatientAppointmentViewModel
    {
        public string FullName { get; set; }
        public Patient patient { get; set; }
        public Doctor Doctors { get; set; }     
        public Staff staff { get; set; }
        public Appointment Appointment { get; set; }
        
    }

}
