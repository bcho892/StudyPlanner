using SQLite;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace StudyPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskList taskList = new TaskList();
        private Timer timerTyping = new Timer(300);

        public MainWindow()
        {
           
            ToRemainingTime.refreshDeadlines(taskList);

            InitializeComponent();
            loadIds();
            tasks.ItemsSource = taskList;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void loadIds()
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                taskList[i].Id += i;
            }
        }

        private void ReverseOrder (object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < taskList.Count; i++)
            {
                taskList.Move(taskList.Count - 1, i);
            }
        }
        private void SortByOrder (object sender, RoutedEventArgs e)
      
            {

                while (true)
                {
                    int sorts = 0;
                    for (int i = 1; i < taskList.Count; i++)
                    {

                        if (taskList[i].Id < taskList[i-1].Id)
                        {
                            taskList.Move(i, i - 1);
                            sorts++;
                        }
                    }
                    if (sorts == 0)
                    {
                        return;
                    }
                }
            }

        private void SortByDeadline(object sender, RoutedEventArgs e)
        {

            int noDeadlineTasks = 0;
            foreach (Task task in taskList)
            {
                if (task.deadline == null)
                    {
                        noDeadlineTasks++;
                    }
            }

            while (true)
            {
                int nullCheck = 0;
                int sorts = 0;
                for (int i = 1; i < taskList.Count; i++)
                {

                    if (taskList[i].deadline == null && taskList[i - 1].deadline == null)
                    {
                        nullCheck++;
                    }else if(taskList[i].deadline != null && taskList[i-1].deadline == null)
                    {
                        taskList.Move(i, i - 1);
                        sorts++;
                    }
                    else if (taskList[i].deadline == null && taskList[i - 1].deadline != null)
                    {
                        nullCheck++;
                    } else if(DateTime.Parse(taskList[i].deadline) < DateTime.Parse(taskList[i - 1].deadline))
                    {
                        taskList.Move(i, i - 1);
                        sorts++;
                    }
                }
                if (sorts == 0 && nullCheck == noDeadlineTasks)
                {
                    return;
                }
            }
        }

        private void SortByUrgency(object sender, RoutedEventArgs e)
        {

            
            while (true)
            {
                int sorts = 0;
                for (int i = 1; i < taskList.Count; i++)
                {

                    if ((int)Enum.Parse(typeof(Priority), taskList[i].priority.ToString()) < (int)Enum.Parse(typeof(Priority), taskList[i-1].priority.ToString()))//enum index concept adapted from https://stackoverflow.com/questions/13257454/get-enum-index-from-string
                    {
                    taskList.Move(i, i - 1);
                    sorts++;
                }
          
            }
            if (sorts == 0)
            {
                return;
            }
        }
    }

    private void SortByName(object sender, RoutedEventArgs e)
        {
            
            while (true) {
                int sorts = 0;
                for (int i = 1; i < taskList.Count; i++)
                {

                if (StringComparer.Ordinal.Compare(taskList[i].name, taskList[i-1].name) < 0)
                {
                    taskList.Move(i, i - 1);
                    sorts++;
                }
                    System.Diagnostics.Debug.WriteLine(taskList[i].name);
            }
            if(sorts == 0)
                {
                    return;
                }
             }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(taskList).Refresh();
            CreateTask createWindow = new CreateTask(taskList);
            createWindow.Show();
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            if (button != null)
            {
                var task = button.DataContext as Task;

                using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
                {
                    connection.Execute("DELETE FROM Task WHERE Name = ?", task.name);
                    connection.Commit();

                }

                taskList.removeTask(task);

            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if(taskList.Count == 0)
            {
                noitems.Visibility = Visibility.Visible;
                return;
            }
            noitems.Visibility = Visibility.Collapsed;
            ToRemainingTime.refreshDeadlines(taskList);
   
        }

        private async void Update(object sender, EventArgs e)
        {

            var textbox = sender as TextBox;
            var task = textbox.DataContext as Task;
            async Task<bool> UserKeepsTyping() // solution adapted from https://stackoverflow.com/questions/33776387/dont-raise-textchanged-while-continuous-typing
            {
                string text = textbox.Text;  
                await System.Threading.Tasks.Task.Delay(300);        
                return text != textbox.Text;  
            }
            if (await UserKeepsTyping()) return;
            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {

                var toReplace = task.name;
                
                switch (textbox.Name)
                {
                    case "Description":
                        toReplace = task.description;
                        break;
                }
                String toExecute = "UPDATE Task SET " + textbox.Name + " = " + "'" + textbox.Text + "'" + " WHERE " + textbox.Name + " = " + "'" + toReplace + "'";
                System.Diagnostics.Debug.WriteLine(textbox.Text);
                connection.Execute(toExecute);
                connection.Commit();
            } 


        }

    }
}
