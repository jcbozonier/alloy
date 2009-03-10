﻿using System.Collections.Generic;
using System.IO;
using System.Windows;
using Unite.UI.Utilities;
using Unite.UI.ViewModels;

namespace Unite.UI.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Loaded += Window1_Loaded;
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            ((ViewModels.IInitializeView)DataContext).Init();
        }

        
    }
}