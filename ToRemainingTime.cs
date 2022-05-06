using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudyPlanner
{
    public class ToRemainingTime 
    {
        public static object Convert(object value)
        {

            try {
                DateTime now = DateTime.Now;
                if (value == null) {
                    return "No Deadline"; 
                }
                DateTime initial = DateTime.Parse(value.ToString());
                TimeSpan remaining = initial.Subtract(now);
                return remaining.ToString("d' Days 'mm' Minutes 'ss' Seconds'").TrimStart(' ', 'd', 'h', 'm', '0'); //adapted from https://stackoverflow.com/questions/5398241/remove-leading-zeros-from-time-to-show-elapsed-time

            }
            catch (FormatException)
            {
                System.Diagnostics.Debug.WriteLine("already formatted");
                return value;
            }
        }
        public static void refreshDeadlines(TaskList taskList)
        {

            foreach(Task task in taskList)
            {
                task.formattedDeadline = (string)Convert(task.deadline);
            }
   

        }

    }
}
