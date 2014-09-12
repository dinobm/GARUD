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
        private DatabaseObjectModel _modelObject;
        private DatabaseObject _modelEntity;

        private List<String> _dbNamesList;
        private String _instanceName;
        private String _catalogName;

        public String CatalogName
        {
            get { return _catalogName; }
            set 
            {
                _catalogName = value;
                if (!String.IsNullOrEmpty(value))
                {
                    GetModelDetails();
                    GetTestCases();
                } 
           }
        }
        private ObservableCollection<TestCase> _testCaseList;

        public ObservableCollection<TestCase> TestCaseList
        {
            get { return _testCaseList; }
            set { _testCaseList = value; RaisePropertyChanged("TestCaseList"); }
        }

        public String InstanceName
        {
            get { return _instanceName; }
            set 
            {
                _instanceName = value; 
                if(!String.IsNullOrEmpty(value))
                {
                    GetModelDetails();
                    GetDBNames();
                }
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
        public DatabaseObjectsViewModel()
        {
            _catalogName = "master";
        }
        private void GetModelDetails()
        {
            var str = new SqlConnectionStringBuilder
            {
                DataSource = InstanceName,
                InitialCatalog = _catalogName,
                IntegratedSecurity = true

            };
            if (!String.IsNullOrEmpty(str.ConnectionString))
                _modelObject = new DatabaseObjectModel(str.ConnectionString);
            if (_modelObject != null)
            {
                _modelEntity = _modelObject.GetDatabaseObjects();
            }
            
        }

        
        private void GetDBNames()
        {
            if (_modelEntity != null)
            {
                DatabaseNames = _modelEntity.DatabaseNamesList;
                
            }
        }

        private void GetTestCases()
        {
            if (_modelEntity != null)
            {
                TestCaseList = _modelEntity.TestCaseList;

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
