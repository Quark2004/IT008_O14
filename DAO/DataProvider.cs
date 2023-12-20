using Npgsql;
using System.Data;
using System.Linq;

namespace QLSV.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }

        public string connectionStr = "Server = localhost; Port = 5433; Database = StudentManagement; User Id = postgres; Password = 123456;";

        public DataTable ExcuteQuery(string query, object[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains(':'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }

                NpgsqlDataReader dataReader = command.ExecuteReader();

                dataTable.Load(dataReader);

                connection.Close();
            }

            return dataTable;
        }

        public int ExcuteNonQuery(string query, object[] parameters = null)
        {
            int data = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains(':'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();
            }

            return data;
        }

        public object ExcuteScalar(string query, object[] parameters = null)
        {
            object data = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains(':'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();
            }

            return data;
        }
    }
}
