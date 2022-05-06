using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media;
/// <summary>
/// Summary description for Class1
/// </summary>
/// 

namespace StudyPlanner
{
    public enum Priority
	{
		URGENT = 0,
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
						Task current = new Task
						{
							name = task.name,
							description = task.description,
							priority = task.priority,
							deadline = task.deadline
						};


						Add(current);

					}
				}
				catch (SQLite.SQLiteException)
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

		public Brush correspondingColour
        {
            get
            {
				switch (this.priority.ToString())
				{
					case "URGENT":
						return (SolidColorBrush)new BrushConverter().ConvertFromString("#C50F1F");
					case "IMPORTANT":
						return (SolidColorBrush)new BrushConverter().ConvertFromString("#C19C00");
					case "AVERAGE":
						return (SolidColorBrush)new BrushConverter().ConvertFromString("#0037DA");
					case "TRIVIAL":
						return (SolidColorBrush)new BrushConverter().ConvertFromString("#13A10E");
				}
				return new SolidColorBrush(Colors.Gray);
			}
        }

    }

}