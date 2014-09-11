using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GARUD.DAL
{
    public class DataAccess
    {
        private readonly string _connectionString = null;
        private SqlConnection _dbConnection = null;

        public DataAccess(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// To get the connection from the existing connection String
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            try
            {
                _dbConnection = new SqlConnection(_connectionString);
                _dbConnection.Open();
                
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
            return _dbConnection;
        }

        /// <summary>
        /// To get list of the tables in the existing connection
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllDatabaseTables()
        {
            var returnList=new List<string>();
            try
            {
                _dbConnection = _dbConnection ?? new SqlConnection(_connectionString);
                _dbConnection.Open();
                if (_dbConnection == null) return returnList;
                var cmd = new SqlCommand("SELECT DISTINCT TABLE_SCHEMA + '.' + TABLE_NAME TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", _dbConnection);
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        returnList.Add(dataReader.GetString(0));
                    }
                }
                return returnList;
            }
            
            finally 
            {
                if(_dbConnection !=null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        /// <summary>
        /// To get list of the tables in the existing connection
        /// </summary>
        /// <returns></returns>
        public List<String> GetAllDatabases()
        {
            var returnList = new List<string>();
            try
            {
                _dbConnection = _dbConnection ?? new SqlConnection(_connectionString);
                if (_dbConnection == null) return returnList;
                _dbConnection.Open();
                var cmd = new SqlCommand("SELECT Name FROM sys.databases where database_id > 4", _dbConnection);
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        returnList.Add(dataReader.GetString(0));
                    }
                }
                return returnList;
            }
            catch (SqlException sqlEx)
            {
                return new List<String> { "Dinesh", "Sample" };
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

    }
}
