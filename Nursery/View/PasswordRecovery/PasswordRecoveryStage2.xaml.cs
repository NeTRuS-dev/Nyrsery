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

namespace Nursery.View.PasswordRecovery
{
    /// <summary>
    /// Логика взаимодействия для PasswordRecoveryStage2.xaml
    /// </summary>
    public partial class PasswordRecoveryStage2 : Window
    {
        public PasswordRecoveryStage2(object dc)
        {
            InitializeComponent();
            DataContext = dc;

        }
    }
}
