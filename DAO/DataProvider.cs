using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class DataProvider
	{
		private string connectionStr = "Server = localhost; Port = 5432; Database = StudentManagement; User Id = postgres; Password = quark1412;";

		public DataTable ExcuteQuery(string query)
		{
			DataTable dataTable = new DataTable();

			using (NpgsqlConnection connection = new NpgsqlConnection(connectionStr))
			{
				connection.Open();
				NpgsqlCommand command = new NpgsqlCommand();
				command.Connection = connection;
				command.CommandType = CommandType.Text;
				command.CommandText = query;
				NpgsqlDataReader dataReader = command.ExecuteReader();
				dataTable.Load(dataReader);
				connection.Close();
			}

			return dataTable;
		}
	}
}
