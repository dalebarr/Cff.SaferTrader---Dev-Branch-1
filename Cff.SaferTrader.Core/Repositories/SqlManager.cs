using System;
using System.Data;
using System.Data.SqlClient;

namespace Cff.SaferTrader.Core.Repositories
{
    public abstract class SqlManager
    {
        private readonly string _connectionString;

        protected SqlManager(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }
            _connectionString = connectionString;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected SqlTransaction CreateTransaction()
        {
            SqlConnection connection = CreateConnection();
            connection.Open();
            return connection.BeginTransaction();
        }

        protected static SqlDataReader ExecuteReader(SqlConnection connection, string spName)
        {
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        protected static SqlDataReader ExecuteReader(SqlConnection connection, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, parameters);
        }

        protected static int ExecuteNonQuery(SqlConnection connection, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(connection, spName, parameters);
        }

        protected static int ExecuteNonQuery(SqlTransaction transaction, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteNonQuery(transaction, spName, parameters);
        }

        protected static object ExecuteScalar(SqlTransaction transaction, string spName)
        {
            return SqlHelper.ExecuteScalar(transaction, spName);
        }

        protected static object ExecuteScalar(SqlConnection connection, string spName)
        {
            return SqlHelper.ExecuteScalar(connection, spName);
        }

        protected static object ExecuteScalar(SqlConnection connection, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteScalar(connection, spName, parameters);
        }

        protected static object ExecuteScalar(SqlTransaction transaction, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteScalar(transaction, spName, parameters);
        }

        protected static DataSet ExecuteDataSet(SqlConnection connection, string spName, SqlParameter[] parameters)
        {
            return SqlHelper.ExecuteDataset(connection, spName, parameters);
        }

        protected static DataSet ExecuteDataSet(SqlConnection connection, string spName)
        {
            return SqlHelper.ExecuteDataset(connection, spName);
        }

        protected static SqlParameter[] AppendSqlParameters(SqlParameter[] firstParameterArray, SqlParameter[] secondParameterArray)
        {
            int firstParameterArrayLength = firstParameterArray.Length;
            int secondParameterArrayLength = secondParameterArray.Length;
            SqlParameter[] parameters = new SqlParameter[firstParameterArrayLength + secondParameterArrayLength];

            firstParameterArray.CopyTo(parameters, 0);
            secondParameterArray.CopyTo(parameters, firstParameterArrayLength);

            return parameters;
        }
    }
}