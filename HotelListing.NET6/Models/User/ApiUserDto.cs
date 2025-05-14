using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.NET6.Models.User
{
	public class ApiUserDto: LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

