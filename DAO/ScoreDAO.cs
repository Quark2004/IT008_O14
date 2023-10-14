using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class ScoreDAO
	{
		private static ScoreDAO instance;
		public static ScoreDAO Instance
		{
			get
			{
				if (instance == null) instance = new ScoreDAO();
				return instance;
			}
			private set
			{
				instance = value;
			}
		}
		private ScoreDAO() { }

		public DataTable GetScore(string id)
		{
			string query = "SELECT * FROM USP_Login( :userName , :password )";

			DataTable res = DataProvider.Instance.ExcuteQuery(query, new object[] { id });

			return res;
		}

		public bool Login(string id)
		{
			return GetScore(id).Rows.Count > 0;
		}
		public int GetRole(string id)
		{
			int res = 0;

			DataTable account = GetScore(id);

			if (account.Rows.Count > 0)
			{
				res = int.Parse(account.Rows[0]["role"].ToString());
			}

			return res;
		}
	}
}
