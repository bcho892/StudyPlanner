using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SQLite;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Threading;
using System.Timers;

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
            tasks.ItemsSource = taskList;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

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

                connection.Execute(toExecute);
                connection.Commit();
            } 


        }

    }
}
