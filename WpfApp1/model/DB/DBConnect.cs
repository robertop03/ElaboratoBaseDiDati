using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace WpfApp1.model.DB
{
    internal class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "localhost";
            database = "StabilimentoBalneare";
            uid = "Roberto";
            password = "progettoDb123";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        #region Open and Close connection
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                    default:
                        break;
                }
                return false;
            }
        }

        private void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        public int Insert(string query, List<MySqlParameter> parameters)
        {
            int rowsAffected = 0;
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters.ToArray());
                rowsAffected = cmd.ExecuteNonQuery();
                CloseConnection();
            }
            return rowsAffected;
        }

        public void Delete(string query, List<MySqlParameter> parameters)
        {
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters.ToArray());
                _ = cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public DataTable Select(string query, List<MySqlParameter> parameters)
        {
            DataTable dataTable = new DataTable();
            if (OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddRange(parameters.ToArray());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                _ = adapter.Fill(dataTable);
                CloseConnection();
            }
            return dataTable;
        }
    }
}
