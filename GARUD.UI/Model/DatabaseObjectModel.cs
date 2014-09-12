using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly DatabaseObject _databaseObject;

        public DatabaseObjectModel(string connectionString)
        {
            _dataAccess = new DataAccess(connectionString);
            _databaseObject = new DatabaseObject();
        }

        public DatabaseObject GetDatabaseObjects()
        {
            try
            {
                _databaseObject.DatabaseNamesList = _dataAccess.GetAllDatabases();
                //Build TestCase List
                GenerateTestCase();
            }
            catch(Exception ex)
            {
                Logger.Log("Exception Log GetDatabaseObjects", ex);
                 _databaseObject.DatabaseNamesList = new List<String> { "Dinesh", "Sample" };
                 _databaseObject.TestCaseList = new ObservableCollection<TestCase>() {new TestCase(){TableName = "SampleTable", SchemaName="dbo", TestCaseName="Check Constraint"} };
                 Logger.Log("BeginProcess: Default Names Loaded");
            }
           
            return _databaseObject;
        }

        private void GenerateTestCase()
        {
            var testCaseCollection = new ObservableCollection<TestCase>();
            try
            {
                var returnTable = _dataAccess.GenerateTestCaseReport();
                if (returnTable != null)
                {
                    foreach (DataRow eachRow in returnTable.Rows)
                    {

                        var testCaseSet = new TestCase()
                        {
                            TableName = Convert.ToString(eachRow["TableName"]),
                            SchemaName = Convert.ToString(eachRow["TableSchema"]),
                            TestCaseName = Convert.ToString(eachRow["TestCase"])
                        };
                        testCaseCollection.Add(testCaseSet);

                    }
                }
                _databaseObject.TestCaseList = testCaseCollection;
            }
            finally
            {

            }
        }
    }
}
