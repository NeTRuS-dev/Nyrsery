using Nursery.Model;
using Nursery.ViewModel;
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

namespace Nursery.View
{
    /// <summary>
    /// Логика взаимодействия для ProfileRedactor.xaml
    /// </summary>
    public partial class ProfileRedactor : Window
    {
       
        public ProfileRedactor(User currentUser)
        {
            InitializeComponent();
            DataContext = new ProfileEdittorViewModel(this, currentUser, false);
        }
        public ProfileRedactor()
        {
            InitializeComponent();

        }
    }
}
