using System.Windows;
using System.Windows.Controls;
using GARUD_UI.ViewModel;

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
            _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
            //_dataViewModel.InstanceName = ".";
         

        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(DatabaseInstanceTextBox.Text))
            {
                _dataViewModel = (DatabaseObjectsViewModel)base.DataContext;
                _dataViewModel.InstanceName = DatabaseInstanceTextBox.Text;
            }
        }
    }

   


}
