using System.Windows;
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
           

        }
       

        private void DatabaseInstanceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(DatabaseInstanceTextBox.Text))
            {
                _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
                _dataViewModel.InstanceName = DatabaseInstanceTextBox.Text;
                _dataViewModel.RefreshScreen();
            }
            if (_dataViewModel.TestCaseList.Count == 0)
            {
                ReportTabs.Visibility = System.Windows.Visibility.Collapsed;
                DisplayMessageBox.Text = "No Test Scenarios can be suggested as GARUD did not find any database constraints to evaluate";
            }
            else
            {
                ReportTabs.Visibility = System.Windows.Visibility.Visible;
                DisplayMessageBox.Text = String.Empty;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DatabaseNamesComboBox.SelectedItem!= null)
            {
                _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
                _dataViewModel.CatalogName = Convert.ToString(DatabaseNamesComboBox.SelectedValue);
                _dataViewModel.RefreshScreen();
            }
            if (_dataViewModel.TestCaseList.Count == 0)
            {
                ReportTabs.Visibility = System.Windows.Visibility.Collapsed;
                DisplayMessageBox.Text = "No Test Scenarios can be suggested as GARUD did not find any database constraints to evaluate";
            }
            else
            {
                ReportTabs.Visibility = System.Windows.Visibility.Visible;
                DisplayMessageBox.Text = String.Empty;
            }
        }

       
    }

   


}
