using System.Globalization;
using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Hospital_Management_System.Models;

namespace demo.Models
{
    public class StaffViewModel: Person
    {
        public string administration_type { get; set; }
        public string Specialization { get; set; }
    }
}
