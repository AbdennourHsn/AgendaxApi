using System;
namespace AgendaxApi.Entities
{
	public class Project
	{
		public int Id { get; set; }
		public string Name {get; set;}
		public string color { get; set; }
		public string logoName {get ; set;}
		public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}

