using Microsoft.AspNetCore.Identity;

namespace Hospital_Management_System.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateOnly BDate { get; set; }
        public string Gender { get; set; }
        public string Blood_Type { get; set; }
        public string Country { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }

    }
}
