using Microsoft.EntityFrameworkCore;
namespace demo.Models
{
	public class HMSEntites : DbContext
	{
		public HMSEntites() : base()
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
			var connectionString = "server=hms-db-hms-db.a.aivencloud.com;port=11793;user=avnadmin;password=AVNS_FZs-lAOyU4XtmPIr-FH;database=HMS-DB";
			var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));
			optionsBuilder.UseMySql(connectionString, serverVersion);
		}
	}
}
