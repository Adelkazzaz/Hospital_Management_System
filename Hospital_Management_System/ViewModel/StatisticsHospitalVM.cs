namespace Hospital_Management_System.ViewModel
{

	public class StatisticsHospitalVM
	{

		public List<DiagnosisAgeItem> AvgAgesByDiagnosis { get; set; }
		public List<TopTreatments> topTreatments { get; set; }
		public int DeathCount { get; set; }
		public string Gender { get; set; }
		public int RiskCount { get; set; }
		public int AppointmentCount { get; set; }
		public string DoctorFullName { get; set; }
		public string Governorate { get; set; }
		public string Diagnosis { get; set; }
		public int DiseaseCount { get; set; }
		public string DepartmentName { get; set; }
		public int PatientCount { get; set; }
		public int Month { get; set; }
		public string Treatment { get; set; }
		public int MedicalCount { get; set; }
		public double AverageSalary { get; set; }
		public string BloodType { get; set; }
		public int DepartmentCount { get; set; }
		public int DoctorCount { get; set; }
		public int NurseCount { get; set; }
	}

	public class DiagnosisAgeItem
	{
		public string Diagnosis { get; set; }
		public double AvgAge { get; set; }
	}
	public class TopTreatments
	{
		public string Treatment { get; set; }
		public int MedicalCount { get; set; }
	}
}
