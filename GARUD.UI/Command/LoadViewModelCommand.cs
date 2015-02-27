using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GARUD_UI.ViewModel;
namespace GARUD_UI.Command
{
    public class LoadViewModelCommand: ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            try
            {
                if (parameter != null)
                {
                    var dataVM = (DatabaseObjectsViewModel)((System.Windows.Controls.DataGrid)parameter).DataContext;

                    if (dataVM != null)
                    {
                        dataVM.GetModelDetails();
                        dataVM.LoadEvaluationResults();
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        
    }
}
