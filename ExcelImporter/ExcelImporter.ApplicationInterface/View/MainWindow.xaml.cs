﻿using ExcelImporter.ApplicationInterface.ViewModel;
using System.Windows;

namespace ExcelImporter.ApplicationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var appViewModel = DataContext as AppViewModel;
            appViewModel?.HandleTreeViewItemSelected(e);
        }
    }
}
