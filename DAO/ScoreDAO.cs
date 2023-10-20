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
			string query = "SELECT * FROM Student_GetScore( :id )";

			DataTable res = DataProvider.Instance.ExcuteQuery(query, new object[] { id });

			return res;
		}

		
	}
}
