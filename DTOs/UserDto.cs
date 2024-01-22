using System;
using System.ComponentModel.DataAnnotations;

namespace AgendaxApi.DTOs
{
	public class UserDto
	{
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Adress { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string AboutMe { get; set; }
        public string gender { get; set; }
    }
	
}

