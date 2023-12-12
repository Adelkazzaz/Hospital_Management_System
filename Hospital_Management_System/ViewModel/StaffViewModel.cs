using System.Globalization;
using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using demo.Models;
using Hospital_Management_System.Models;


namespace Hospital_Management_System.ViewModel
{
    public class StaffViewModel : Staff
    {
        
		public string Specialization { get; set; }
		public Nurse Nurse { get; set; }
        public Administrator Admin { get; set; }
        public int NoOfDepartment { get; set; }
        public int NoOfDoctors { get; set; }
        public int NoOfPatients { get; set; }
        public int NoOfNurses { get; set; }
    }
}
