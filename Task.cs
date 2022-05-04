using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace StudyPlanner
{
	public enum Priority
	{
		URGENT,
		IMPORTANT,
		AVERAGE,
		TRIVIAL
	}

	public class TaskList : ObservableCollection<Task>
	{	

		public TaskList() : base()
        {
			using (SQLiteConnection conn = new SQLiteConnection(App.dbPath))
			{
				var data = conn.Table<Task>();
				
				try
				{
					foreach (var task in data)
					{
						System.Diagnostics.Debug.WriteLine("Stock: " + task.name);
						Task current = new Task
						{
							name = task.name,
							description = task.description,
							priority = task.priority
						};

						System.Diagnostics.Debug.WriteLine("Stock: " + current.priority);


						Add(current);

					}
				}catch(SQLite.SQLiteException e)
                {

					Debug.WriteLine("No db file found!");
					return;
                }

			}
		}
		public void addTasks(Task task)
        {
			Add(task);
        }
    }

	public class Task 
	{

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Column("Name")]
		public String name { get; set; }

		[Column("Description")]
		public String description { get; set; }

		[Column("Priority")]
		public Priority priority { get; set;}



    }

}