﻿using Nursery.Model;
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

namespace Nursery.View.AdminFrames.Helpers
{
    /// <summary>
    /// Логика взаимодействия для UsersTab.xaml
    /// </summary>
    public partial class UsersTab : Page
    {
        public UsersTab(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;

        }
    }
}
