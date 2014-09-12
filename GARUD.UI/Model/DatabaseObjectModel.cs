using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GARUD.Common;
using GARUD.DAL;
using GARUD.Entity;

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
            }
            catch(Exception ex)
            {
                Logger.Log("Exception Log GetDatabaseObjects", ex);
                 _databaseObject.DatabaseNamesList = new List<String> { "Dinesh", "Sample" };
                 Logger.Log("BeginProcess: Default Names Loaded");
            }
           
            return _databaseObject;
        }
    }
}
