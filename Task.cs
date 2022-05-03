using System;
using System.Globalization;
/// <summary>
/// Summary description for Class1
/// </summary>
public enum Priority
{
	URGENT,
	IMPORTANT,
	AVERAGE,
	TRIVIAL
}

public class Task {
	
		private DateTime deadline { get; set; }
		private Priority priority { get; set; }
		private string description { get; set; }
		
		public Task(Priority priority, DateTime deadline, String description)
		{
		this.priority = priority;
		this.deadline = deadline;
		this.description = description;
		}


}
