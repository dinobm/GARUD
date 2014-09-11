using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GARUD.Entity;
using GARUD_UI.Model;
using System.ComponentModel;


namespace GARUD_UI.ViewModel
{
    public class DatabaseObjectsViewModel : INotifyPropertyChanged
    {
        private DatabaseObjectModel _model;

        private DatabaseObject _entity;

        private List<String> _dbNamesList;
        private String _instanceName;

        public String InstanceName
        {
            get { return _instanceName; }
            set 
            {
                _instanceName = value; 
                if(!String.IsNullOrEmpty(value))
                {
                    InitModel();
                }
            }
        }

        private void InitModel()
        {
            var str = new SqlConnectionStringBuilder
            {
                DataSource = InstanceName,
                InitialCatalog = "master",
                IntegratedSecurity = true

            };
            if (!String.IsNullOrEmpty(str.ConnectionString))
                _model = new DatabaseObjectModel(str.ConnectionString);
            if (_model != null)
            {
                _entity = _model.GetDatabaseObjects();
            }
            if (_entity != null)
            {
                DatabaseNames = _entity.DatabaseNamesList;
            }
        }

        public DatabaseObjectsViewModel()
        {
            _instanceName = ".";
            InitModel();
        }

        
        //public void LoadDbNames()
        //{
           
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> DatabaseNames
        {
            get { return _dbNamesList; }
            set
            {

                _dbNamesList = value;
                RaisePropertyChanged("DatabaseNames");

            }
        }

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
