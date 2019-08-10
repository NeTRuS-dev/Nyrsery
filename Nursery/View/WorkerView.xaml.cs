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
using Nursery.Model;
using Nursery.ViewModel;

namespace Nursery.View
{
    /// <summary>
    /// Логика взаимодействия для WorkerView.xaml
    /// </summary>
    public partial class WorkerView : Window
    {
        internal WorkerView(User currentUser)
        {
            InitializeComponent();
            DataContext = new WorkerViewModel(this, currentUser);
        }
    }
}
