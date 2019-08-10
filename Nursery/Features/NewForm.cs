
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Nursery.Model;

namespace Nursery.ViewModel
{
    public class Form
    {

        public static void Create(Window sender, Window newWindow)
        {
            Pet.SelectedPet = null;
            newWindow.Left = sender.Left;
            newWindow.Top = sender.Top;
            newWindow.Width = sender.Width;
            newWindow.Height = sender.Height;
            newWindow.WindowState = sender.WindowState;
            newWindow.Show();
            sender.Close();
        }

    }
}
