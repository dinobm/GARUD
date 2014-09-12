using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
        }

        public DataTable GenerateTestCaseReport()
        {
            var retrunTable = new DataTable();            
            try
            {
                _dbConnection = _dbConnection ?? new SqlConnection(_connectionString);
                if (_dbConnection == null) return retrunTable;
                _dbConnection.Open();
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("; WITH GARUD AS ( SELECT  COL.TABLE_NAME, TAB.TABLE_TYPE, TAB.TABLE_SCHEMA, COL.COLUMN_NAME, ");
                queryBuilder.Append(" COL.CHARACTER_MAXIMUM_LENGTH, COL.CHARACTER_OCTET_LENGTH, IS_NULLABLE, DATA_TYPE, TABCONS.CONSTRAINT_TYPE, TABCONS.CONSTRAINT_NAME  FROM");
	            queryBuilder.Append(" INFORMATION_SCHEMA.COLUMNS COL INNER JOIN  (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE') TAB ON");
                queryBuilder.Append(" COl.TABLE_NAME= TAB.TABLE_NAME AND	COL.TABLE_SCHEMA = TAB.TABLE_SCHEMA LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE COLCONS");
                queryBuilder.Append(" ON COL.COLUMN_NAME	 = COLCONS.COLUMN_NAME  AND	COL.TABLE_NAME = COLCONS.TABLE_NAME AND	TAB.TABLE_SCHEMA = COLCONS.TABLE_SCHEMA");
                queryBuilder.Append(" LEFT OUTER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TABCONS ON COLCONS.TABLE_NAME = TABCONS.TABLE_NAME AND COLCONS.CONSTRAINT_NAME = TABCONS.CONSTRAINT_NAME");
                queryBuilder.Append(" AND COLCONS.TABLE_SCHEMA = TABCONS.TABLE_SCHEMA ) , PrimaryKeyValidator AS ( SELECT TABLE_NAME,TABLE_SCHEMA, COLUMN_NAME = ");
                queryBuilder.Append(" STUFF((SELECT ', ' + COLUMN_NAME FROM GARUD b WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND b.TABLE_NAME = a.TABLE_NAME  AND b.TABLE_SCHEMA ");
                queryBuilder.Append(" = a.TABLE_SCHEMA FOR XML PATH('')), 1, 2, '') FROM GARUD a WHERE CONSTRAINT_TYPE='PRIMARY KEY' GROUP BY TABLE_NAME,TABLE_SCHEMA  )");
                queryBuilder.Append(" , ForeignKeyValidator AS (SELECT TABLE_NAME,TABLE_SCHEMA, COLUMN_NAME =  STUFF((SELECT ', ' + COLUMN_NAME FROM GARUD b WHERE ");
                queryBuilder.Append(" CONSTRAINT_TYPE='FOREIGN KEY' AND b.TABLE_NAME = a.TABLE_NAME  AND b.TABLE_SCHEMA = a.TABLE_SCHEMA FOR XML PATH('')), 1, 2, '')");
                queryBuilder.Append(" FROM GARUD a WHERE CONSTRAINT_TYPE='FOREIGN KEY' GROUP BY TABLE_NAME,TABLE_SCHEMA  )");
                queryBuilder.Append(" , uniqueValidator AS ( SELECT TABLE_NAME,TABLE_SCHEMA, COLUMN_NAME = STUFF((SELECT ', ' + COLUMN_NAME FROM GARUD b WHERE");
                queryBuilder.Append(" CONSTRAINT_TYPE='UNIQUE' AND b.TABLE_NAME = a.TABLE_NAME  AND b.TABLE_SCHEMA = a.TABLE_SCHEMA AND a.CONSTRAINT_NAME = b.CONSTRAINT_NAME");
                queryBuilder.Append(" FOR XML PATH('')), 1, 2, '') FROM GARUD a WHERE CONSTRAINT_TYPE='UNIQUE' GROUP BY TABLE_NAME,TABLE_SCHEMA, CONSTRAINT_NAME )");
                queryBuilder.Append(" , GARUDMatrix AS ( SELECT TABLE_SCHEMA, Table_Name, 'Check Primary Key for ' + COLUMN_NAME + ' in the ' + TABLE_SCHEMA + '.'+ TABLE_NAME TestCase ");
                queryBuilder.Append(" FROM PrimaryKeyValidator UNION SELECT TABLE_SCHEMA, Table_Name, ");
                queryBuilder.Append("'Check Foreign Key for ' + COLUMN_NAME + ' in the ' + TABLE_SCHEMA + '.'+ TABLE_NAME FROM ForeignKeyValidator ");
                queryBuilder.Append(" UNION SELECT TABLE_SCHEMA, Table_Name, 'Check Unique Constraint For ' + COLUMN_NAME + ' in the ' + TABLE_SCHEMA + '.'+ TABLE_NAME FROM uniqueValidator )");
                queryBuilder.Append("SELECT TABLE_SCHEMA TableSchema, Table_Name TableName, TestCase FROM GARUDMatrix ORDER BY TABLE_SCHEMA, Table_Name");
                var cmd = new SqlCommand(queryBuilder.ToString(), _dbConnection);
                var dataAdapt = new SqlDataAdapter();
                dataAdapt.SelectCommand = cmd;
                dataAdapt.Fill(retrunTable);
                
            }
            finally
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                    _dbConnection.Close();
            }
            return retrunTable;
        }



    }
}
