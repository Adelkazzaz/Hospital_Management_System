using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demo.Models
{
    public class Nurce
    {
        [Key, ForeignKey("Staff")]
        public int Id { get; set; }
        public string Specialization { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual List<Patient> Patients { get; set; }
    }
}
