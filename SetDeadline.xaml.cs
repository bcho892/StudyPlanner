using System;
using System.Windows;

namespace StudyPlanner
{
    /// <summary>
    /// Interaction logic for SetDeadline.xaml
    /// </summary>
    public partial class SetDeadline : Window
    {
        private Task task;
        
        public SetDeadline(Task task)
        {
            this.task = task;
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!checkDateValidity(DeadlineDate.DateTime.Value))
            {
                return;
             
            }
            else
            {
                var time = DeadlineTime.Value.Value.TimeOfDay;
                task.deadline = DeadlineDate.DateTime.Value.Date.Add(time).ToString();
                System.Diagnostics.Debug.WriteLine(task.deadline.ToString());
                this.Close();
               
            }
            
        }

        private Boolean checkDateValidity(DateTime givenDateTime)
        {
            return givenDateTime.CompareTo(DateTime.Now) < 0 ? false : true;
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
