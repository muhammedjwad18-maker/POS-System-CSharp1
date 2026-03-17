using System;
using System.Data;
using System.Data.SqlClient;

namespace InventorySystem
{
    public partial class DatabaseHelper
    {
        private static DatabaseHelper _instance;
        private SqlConnection connection;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Hi\Documents\InventorySystem.mdf;Integrated Security=True;Connect Timeout=30;";

        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseHelper();
                }
                return _instance;
            }
        }

        private DatabaseHelper()
        {
            connection = new SqlConnection(connectionString);
        }

        public SqlConnection GetConnection()
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"هەڵە لە جێبەجێکردنی داواکاری: {ex.Message}");
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(query, GetConnection()))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"هەڵە لە جێبەجێکردنی داواکاری: {ex.Message}");
            }
            return rowsAffected;
        }

        public SqlParameter CreateParameter(string name, object value, SqlDbType type, int size = 0)
        {
            SqlParameter parameter = new SqlParameter(name, type);
            parameter.Value = value ?? DBNull.Value;

            if (size > 0)
                parameter.Size = size;

            return parameter;
        }

        public void Dispose()
        {
            if (connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }

        ~DatabaseHelper()
        {
            Dispose();
        }
    }
}