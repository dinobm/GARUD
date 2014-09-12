﻿using System.Windows;
using System.Windows.Controls;
using GARUD_UI.ViewModel;
using System.Threading;
using System;

namespace GARUD_UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseObjectsViewModel _dataViewModel ;

        public MainWindow()
        {
            InitializeComponent();
            //_dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
            //_dataViewModel.InstanceName = ".";
         

        }
       

        private void DatabaseInstanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DatabaseInstanceTextBox.Text))
            {
                _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
                _dataViewModel.InstanceName = DatabaseInstanceTextBox.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DatabaseNamesComboBox.SelectedItem!= null)
            {
                _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
                _dataViewModel.CatalogName = Convert.ToString(DatabaseNamesComboBox.SelectedValue);
            }
            if (_dataViewModel.TestCaseList.Count == 0)
            {
                TestCaseSet.Visibility = System.Windows.Visibility.Collapsed;
                DisplayMessageBox.Text = "No Test Scenarios can be suggested as GARD did not find any database constraints to evaluate";
            }
            else
            {
                TestCaseSet.Visibility = System.Windows.Visibility.Visible;
            }
        }

       
    }

   


}
