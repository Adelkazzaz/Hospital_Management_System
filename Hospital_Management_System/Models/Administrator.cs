using Hospital_Management_System.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo.Models
{
	public class Administrator
	{
		[Key, ForeignKey("Staff")]
		public int Id { get; set; }
		public string Specialty { get; set; }
		public string Position { get; set; }

		public virtual Staff Staff { get; set; }
		public virtual Hospital Hospital { get; set; }

	}
}
