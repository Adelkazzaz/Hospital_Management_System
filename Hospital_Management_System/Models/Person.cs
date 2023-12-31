﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
namespace Hospital_Management_System.Models
{
	public class Person
    {
		[Key]
		public int ID { get; set; }
		public string FullName { get; set; }
		public string Gender { get; set; }
		public DateOnly BDate { get; set; }
		public string Phone { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Country { get; set; }
		public string Governorate { get; set; }
		public string City { get; set; }
	}
}
