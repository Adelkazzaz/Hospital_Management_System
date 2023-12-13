using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class Department
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Office { get; set; }

		[ForeignKey("Hospital")]
		public int Hospital_id { get; set; }
		public virtual Hospital Hospital { get; set; }

		public virtual ICollection<Staff> StaffList { get; set; }
	}
}