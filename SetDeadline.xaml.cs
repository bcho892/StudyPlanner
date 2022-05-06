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
            var time = DeadlineTime.Value.Value.TimeOfDay;
            if (!checkDateValidity(DeadlineDate.DateTime.Value.Date.Add(time)))
            {
                warning.Visibility= Visibility.Visible;
                return;
             
            }
            else
            {
                
                task.deadline = DeadlineDate.DateTime.Value.Date.Add(time).ToString();
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
