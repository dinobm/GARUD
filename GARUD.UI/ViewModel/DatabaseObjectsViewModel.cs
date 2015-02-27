using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GARUD.Entity;
using GARUD_UI.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace GARUD_UI.ViewModel
{
    public class DatabaseObjectsViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseObjectModel _modelObject;
        private List<String> _dbNamesList;
        private String _instanceName;
        private String _catalogName;      
        private ObservableCollection<TablesValidation> _testCaseList;
        private string _displayMessage;
        private ObservableCollection<ColumnDesignCheck> _columnDesignEvaluation;
        public ObservableCollection<ColumnDesignCheck> ColumnDesignEvaluation
        {
            get { return _columnDesignEvaluation; }
            set
            {
                _columnDesignEvaluation = value;
                
            }
        }
        public ObservableCollection<TablesValidation> TestCaseList
        {
            get { return _testCaseList; }
            set { _testCaseList = value;  }
        }

        public String CatalogName
        {
            get { return _catalogName; }
            set
            {
                _catalogName = value;
                if (_catalogName != null)
                {
                    RaisePropertyChanged("CatalogName");
                }

            }
        }

        public String InstanceName
        {
            get { return _instanceName; }
            set
            {
                _instanceName = value;
              
               RaisePropertyChanged("InstanceName");

            }
        }

        public List<string> DatabaseNames
        {
            get { return _dbNamesList; }
            set
            {

                _dbNamesList = value;
                RaisePropertyChanged("DatabaseNames");

            }
        }

        public string DisplayMessage
        {
            get { return _displayMessage; }
            set
            {
                _displayMessage = value;
                RaisePropertyChanged("DisplayMessage");
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseObjectsViewModel()
        {
            DisplayMessage = string.Empty;
            //init to default values
            InstanceName = ".";
            CatalogName = string.Empty;
            _modelObject = new DatabaseObjectModel();
            TestCaseList = new ObservableCollection<TablesValidation>();
            ColumnDesignEvaluation = new ObservableCollection<ColumnDesignCheck>();
            GetCatalogDetails();
            LoadEvaluationResults();
        }

        public void GetCatalogDetails()
        {
            var str = new SqlConnectionStringBuilder
            {
                DataSource = InstanceName,
                InitialCatalog = String.IsNullOrEmpty(CatalogName) ? "ReportServer" : CatalogName,
                IntegratedSecurity = true

            };
            if (!String.IsNullOrEmpty(str.ConnectionString))
                
            if (_modelObject != null)
            {
                _modelObject.GetCatalogInfo(str.ConnectionString);
                DatabaseNames = _modelObject.DbNamesList;
               
            }

        }

        public void LoadEvaluationResults()
        {
            TestCaseList.Clear();
            ColumnDesignEvaluation.Clear();
            var str = new SqlConnectionStringBuilder
            {
                DataSource = InstanceName,
                InitialCatalog = String.IsNullOrEmpty(CatalogName) ? "ReportServer" : CatalogName,
                IntegratedSecurity = true

            };
            if (!String.IsNullOrEmpty(str.ConnectionString))

                if (_modelObject != null)
                {
                    

                    _modelObject.RunValidationReport(str.ConnectionString);
                    //Add each item from Model to TC Collection
                    _modelObject.ValidationList.ForEach(TestCaseList.Add);
                    //Add each item from Model to Column Validation
                    _modelObject.ColumnDesignEvaluation.ForEach(ColumnDesignEvaluation.Add);

                }
            DisplayMessage = "Evaluation results shown below";
            if (TestCaseList.Count == 0)
            {
                DisplayMessage = "The tool did not identify any constraints to evaluate database";
            }
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }


}
