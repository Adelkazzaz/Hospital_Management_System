using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace demo.Models
{
	public class HMSEntites : IdentityDbContext<ApplicationUser>
	{
		public HMSEntites() : base()
		{
		}
        public HMSEntites(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Staff> Staffs { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Nurse> Nurses { get; set; }
		public DbSet<Administrator> Administrators { get; set; }
		public DbSet<Hospital> Hospitals { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<MedicalRecord> MedicalRecords { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var connectionString = "server=localhost;user=root;password=root;database=HMS-DB";
			var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));
			optionsBuilder.UseMySql(connectionString, serverVersion);
		}
	}
}
