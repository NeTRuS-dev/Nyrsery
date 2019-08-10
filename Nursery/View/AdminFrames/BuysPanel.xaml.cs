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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nursery.Model;

namespace Nursery.View.AdminFrames
{
    /// <summary>
    /// Логика взаимодействия для BuysPanel.xaml
    /// </summary>
    public partial class BuysPanel : Page
    {
        public BuysPanel(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

    }
}
