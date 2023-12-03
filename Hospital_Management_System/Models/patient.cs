
using Hospital_Management_System.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace demo.Models
{
    public class Patient : Person 
    {
        
        [ForeignKey("Nurce")]
        public int Nurce_id { get; set; }
        public virtual Nurce Nurce { get; set; }

        public virtual MedicalRecord MedicalRecord { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
    }
}
