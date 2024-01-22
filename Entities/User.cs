using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;

namespace AgendaxApi.Entities
{
	public class User
	{
		[Key]
		public int Id;
        [Required]
		public string UserName { get; set; }
        [Required]
		public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string gender { get; set; }
        public string Adress {get ; set; }
		public string Country { get; set; }
		public string PostCode { get; set; }
        public string AboutMe { get; set; }

        public ICollection<Project> Projects {get ; set;}
        public ICollection<Todo> ToDos { get; set; }
    }
}

