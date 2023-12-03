using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        [ForeignKey("Administrator")]
        public int Administrator_id { get; set; }
        public virtual Administrator Administrator { get; set; }
        public virtual List<Staff> StaffList { get; set; }
    }

}