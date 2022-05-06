using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
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



	public class TaskList : BindingList<Task>
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
						Task current = new Task
						{
							name = task.name,
							description = task.description,
							priority = task.priority,
							deadline = task.deadline
						};

						System.Diagnostics.Debug.WriteLine("Stock: " + current.priority);


						Add(current);

					}
				}catch(SQLite.SQLiteException)
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

		public void removeTask(Task task)
        {
			Remove(task);
        }

		
    }

	public class Task : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Column("Name")]
		public String name { get; set; }

		[Column("Description")]
		public String description { get; set; }

		[Column("Priority")]
		public Priority priority { get; set; }

		[Column("Deadline")]
		public String deadline { get; set; }

		private String formattedDeadlineValue;
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public String formattedDeadline { get
			{
				return this.formattedDeadlineValue;
			}
			set
			{
				if (value != this.formattedDeadlineValue)
				{
					this.formattedDeadlineValue = value;
					NotifyPropertyChanged();
				}
			}
		}



    }

}