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

namespace StudyPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskList taskList = new TaskList();
        public MainWindow()
        {
            InitializeComponent();
            tasks.ItemsSource = taskList;

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(taskList).Refresh();
            CreateTask createWindow = new CreateTask(taskList);
            createWindow.Show();
        }

        private void tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

    }
}
