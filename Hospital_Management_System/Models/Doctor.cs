using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace demo.Models
{
    public class Doctor
    {
        [Key, ForeignKey("Staff")]
        public int Id { get; set; }
        public string Specialization { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual List<Appointment> Appointments { get; set; }
    }
}