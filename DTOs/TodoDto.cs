using System;
using AgendaxApi.Entities;

namespace AgendaxApi.DTOs
{
	public class TodoDto
	{
        public int Id {get ; set;}
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime DeadTime { get; set; }
        public TodoState State { get; set; }
        public string Color { get; set; }
        public int ProjectId {get;set;}
        
    }
}

