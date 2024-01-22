using System;
namespace AgendaxApi.Entities
{
	public class Todo
	{
		public int Id { get; set; }
		public string Title {get; set;}
		public string Description { get; set; }
		public DateTime Date { get; set; }
		public DateTime DeadTime { get; set; }
		public TodoState? State { get; set; }
		public string Color { get; set; }
        public int UserId { get; set; } 

		public int ProjectId{get ; set;}

		public virtual Project Project {get ; set;}
        public virtual User User { get; set; }

    }


	public enum TodoState
	{
		todo,
		inProsess,
		done
	}
}

