using System;
using System.Collections.Generic;
using GARUD.Common;
using GARUD.DAL;
using GARUD.Entity;
using System.Collections.ObjectModel;
using System.Data;

namespace GARUD_UI.Model
{
    public class DatabaseObjectModel
    {
        private readonly DataAccess _dataAccess;
        private List<String> _dbNamesList;

        public List<String> DbNamesList
        {
            get { return _dbNamesList; }
            set { _dbNamesList = value; }
        }

        private List<TablesValidation> _validationList;

        public List<TablesValidation> ValidationList
        {
            get { return _validationList; }
            set { _validationList = value; }
        }

        private List<ColumnDesignCheck> _columnDesignEvaluation; 

        public List<ColumnDesignCheck> ColumnDesignEvaluation
        {
            get { return _columnDesignEvaluation; }
            set { _columnDesignEvaluation = value; }
        }

        public void GetCatalogInfo(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _dataAccess.ConnectionString = connectionString;
                 _dbNamesList = _dataAccess.GetAllDatabases();
            }
        }

        public DatabaseObjectModel()
        {
            _dataAccess = new DataAccess();
             //Initialize the model lists
            _columnDesignEvaluation = new List<ColumnDesignCheck>();
            _validationList = new List<TablesValidation>();
        }

        public void RunValidationReport(string connectionString)
        {
            try
            {
                _dataAccess.ConnectionString = connectionString;
                if (_dbNamesList != null)
                    BuildEvaluationResults();               
            }
            catch(Exception ex)
            {
                Logger.Log("Exception Log GetDatabaseObjects", ex);
                //_dbNamesList = new List<String> { "Dinesh", "Sample" };
                //_testCaseList = new ObservableCollection<TablesTestCase>() { new TablesTestCase() { TableName = "SampleTable", SchemaName = "dbo", TestCaseName = "Check Constraint" } };
                 Logger.Log("BeginProcess: Default Names Loaded");
            }           
           
        }

        private void BuildEvaluationResults()
        {
            try
            {
                _validationList.Clear();
                _columnDesignEvaluation.Clear();
                var returnTable = _dataAccess.GenerateValidationReport("TestCaseQuery");
                if (returnTable != null)
                {
                    foreach (DataRow eachRow in returnTable.Rows)
                    {

                        var testCaseSet = new TablesValidation
                        {
                            TableName = Convert.ToString(eachRow["TableName"]),
                            SchemaName = Convert.ToString(eachRow["TableSchema"]),
                            TestCaseName = Convert.ToString(eachRow["TestCase"])
                        };
                        _validationList.Add(testCaseSet);

                    }
                }
               //Run through COlumn Checks now
                returnTable = _dataAccess.GenerateValidationReport("DesignCheckQuery");
                if (returnTable != null)
                {
                    foreach (DataRow eachRow in returnTable.Rows)
                    {

                        var designEvalSet = new ColumnDesignCheck
                        {
                            TableName = Convert.ToString(eachRow["TableName"]),
                            SchemaName = Convert.ToString(eachRow["TableSchema"]),
                            ColumnName = Convert.ToString(eachRow["ColumnName"]),
                            PrimaryColumnName = Convert.ToString(eachRow["BaseColName"]),
                            NullableFieldMismatch = Convert.ToString(eachRow["NullableMismatch"]),
                            MaxSizeMismatch = Convert.ToString(eachRow["MaxSizeMismatch"]),
                            OctetSizeMismatch = Convert.ToString(eachRow["OctetSizeMismatch"]),
                            DataTypeMismatch = Convert.ToString(eachRow["DatatypeMismatch"])
                        };
                        _columnDesignEvaluation.Add(designEvalSet);

                    }
                }
                
            }
            finally
            {

            }
        }
    }
}
