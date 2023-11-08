using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class AccountDAO
	{
		private static AccountDAO instance;
		public static AccountDAO Instance
		{
			get
			{
				if (instance == null) instance = new AccountDAO();
				return instance;
			}
			private set
			{
				instance = value;
			}
		}
		private AccountDAO() { }

		public DataTable GetAccount(string userName, string password)
		{
			string query = "SELECT * FROM \"Login\"( :userName , :password )";

			DataTable res = DataProvider.Instance.ExcuteQuery(query, new object[] { userName, password });

			return res;
		}

		public bool Login(string userName, string password)
		{
			return GetAccount(userName, password).Rows.Count > 0;
		}
		public string GetRole(string userName, string password)
		{
			string res = "";

			DataTable account = GetAccount(userName, password);

			if (account.Rows.Count > 0)
			{
				res = account.Rows[0]["role"].ToString();
			}

			return res;
		}
		public string GetId(string username, string password)
		{
			string res = "";

			DataTable account = GetAccount(username, password);

			if (account.Rows.Count > 0)
			{
				res = account.Rows[0]["id"].ToString();
			}

			return res;
		}
	}
}
