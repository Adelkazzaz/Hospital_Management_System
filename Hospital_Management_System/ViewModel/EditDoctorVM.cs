using demo.Models;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.ViewModel
{
    public class EditDoctorVM
    {
        private string _specialization;
        private string _qualifications;
        private string _governorate;
        private string _city;

        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength= 5, ErrorMessage = "Full name must be between 5 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z]+(?:[' -][a-zA-Z]+)$", ErrorMessage = "Invalid full name, You should write first name , one space and last name")]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 4, ErrorMessage = "Role must be between 4 and 10 characters.")]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"\d+$", ErrorMessage = "Invalid Salary,You should write numuric value only.")]

        public int Salary { get; set; }
        [Required]
        public DateOnly BDate { get; set; }
        [Required]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid Phone Number,You should write Egyption Phone number.")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email.")]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Password must be more than 6 characters.")]
        public string Password { get; set; }
        

        [Display(Name = "Department")]
        [Required]
        public int DeptId { get; set; }
        

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Specialization must be between 3 and 50 characters.")]
        public string Specialization
        {
            get => _specialization;
            set => _specialization = new HtmlSanitizer().Sanitize(value);
        }

        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Qualifications must be between 3 and 50 characters.")]
        [Required]

        public string Qualifications
        {
            get => _qualifications;
            set => _qualifications = new HtmlSanitizer().Sanitize(value);
        }


        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Governorate must be between 3 and 50 characters.")]
        public string Governorate
        {
            get => _governorate;
            set => _governorate =new HtmlSanitizer().Sanitize(value);
        }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "City must be between 3 and 50 characters.")]
        public string City
        {
            get => _city;
            set => _city = new HtmlSanitizer().Sanitize(value);
        }

		
		public string Shift { get; set; }
		
		public string Country { get; set; }

	}
}
