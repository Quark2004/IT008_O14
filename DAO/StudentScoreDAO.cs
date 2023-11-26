using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class StudentScoreDAO
	{
		private static StudentScoreDAO instance;

		public static StudentScoreDAO Instance
		{
			get
			{
				if (instance == null) instance = new StudentScoreDAO();
				return StudentScoreDAO.instance;
			}
			private set
			{
				StudentScoreDAO.instance = value;
			}
		}

		private StudentScoreDAO() { }

		public List<StudentScore> LoadStudentScore(string id)
		{
			string query = "SELECT * FROM GetLearningOutcomes( :id )";
			DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
			List<StudentScore> scores = new List<StudentScore>();

			foreach (DataRow row in data.Rows)
			{
				StudentScore score = new StudentScore(row);
				scores.Add(score);
			}

			return scores;
		}
	}
}
