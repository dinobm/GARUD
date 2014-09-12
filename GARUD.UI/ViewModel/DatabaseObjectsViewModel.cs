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

        private void GetModelDetails()
        {
            var str = new SqlConnectionStringBuilder
            {
                DataSource = InstanceName,
                InitialCatalog = "master",
                IntegratedSecurity = true

            };
            if (!String.IsNullOrEmpty(str.ConnectionString))
                _modelObject = new DatabaseObjectModel(str.ConnectionString);
            if (_modelObject != null)
            {
                _modelEntity = _modelObject.GetDatabaseObjects();
            }
            if (_modelEntity != null)
            {
                DatabaseNames = _modelEntity.DatabaseNamesList;
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
