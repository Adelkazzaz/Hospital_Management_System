using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.ViewModel
{
    public class RegisterUserViewModel
    {
        /*=================================*/
        public required string FullName { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public DateOnly BDate { get; set; }
        /*=================================*/
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        /*=================================*/
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }


    }
}
