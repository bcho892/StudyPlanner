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
    /// Interaction logic for SetDeadline.xaml
    /// </summary>
    public partial class SetDeadline : Window
    {
        private Task task;
        public SetDeadline(Task task)
        {
            InitializeComponent();
            
        }
    }
}
