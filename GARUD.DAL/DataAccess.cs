using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace GARUD.DAL
{
    public class DataAccess
    {
        private string _connectionString;
        

        public DataAccess()
        {
            ConnectionString = string.Empty;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
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
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var cmd =
                        new SqlCommand(
                            "SELECT DISTINCT TABLE_SCHEMA + '.' + TABLE_NAME TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'",
                            dbConnection);
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(dataReader.GetString(0));
                        }
                    }
                   
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("GetAllDatabaseTables - Error while trying to connect to database" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return returnList;
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
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    
                    var cmd = new SqlCommand("SELECT Name FROM sys.databases where database_id > 4", dbConnection);
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            returnList.Add(dataReader.GetString(0));
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("GetAllDatabases - Error while trying to connect to database" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return returnList;
        }
        //DesignCheckQuery, or TestCaseQuery
        public DataTable GenerateValidationReport(string inputQueryType)
        {
            var retrunTable = new DataTable();            
            try
            {
                if (String.IsNullOrEmpty(inputQueryType))
                    throw new ArgumentNullException(inputQueryType, "Parameter cannot benull or empty");
                using (var dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
                    var queryToExecute = Convert.ToString(ConfigurationManager.AppSettings[inputQueryType]);
                    using (var cmd = new SqlCommand(queryToExecute, dbConnection))
                    {
                        using(var dataAdapt = new SqlDataAdapter())
                        {
                            dataAdapt.SelectCommand = cmd;
                            dataAdapt.Fill(retrunTable);
                        }
                    }

                    
                    
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("GenerateTestCaseReport - Error while trying to connect to database - " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return retrunTable;
            
        }

    }
}
