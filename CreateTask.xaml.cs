
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudyPlanner
{
    /// <summary>
    /// Interaction logic for CreateTask.xaml
    /// </summary>
    /// 
    
    public partial class CreateTask : Window
    {
        private TaskList taskList; 
        private Task currentTask = new Task();
        public CreateTask(TaskList taskList)
        {
            InitializeComponent();
            TaskPriority.SelectedIndex = 3;
            this.taskList = taskList;
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (TaskName.Text.Replace(" ", "") == "")
            {
                warning.Visibility = Visibility.Visible;
                return;
            }

            Priority priority;
            Enum.TryParse(TaskPriority.Text, out priority);
            
            currentTask.name = TaskName.Text;
            currentTask.priority = priority;
            currentTask.description = TaskDescription.Text;

            System.Diagnostics.Debug.WriteLine(priority);

            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {
                connection.CreateTable<Task>();
                connection.Insert(currentTask);
            }

            taskList.addTasks(currentTask);
            this.Close();

        }

        private void SetDeadline(object sender, RoutedEventArgs e)
        {
            SetDeadline setDeadline = new SetDeadline(currentTask);
            setDeadline.Show();
            
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
