using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class StudentInfoDAO
	{
		private static StudentInfoDAO instance;

		public static StudentInfoDAO Instance 
		{ 
			get
			{
				if (instance == null) instance = new StudentInfoDAO();
				return StudentInfoDAO.instance;
			} 
			private set
			{
				StudentInfoDAO.instance = value;
			}
		}

		private StudentInfoDAO() { }

		public StudentInfo LoadStudentInfo(string id)
		{
			string query = "SELECT * FROM LoadProfileById( :id )";
			DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
			StudentInfo info = new StudentInfo(data.Rows[0]);
			
			return info;
		}
	}
}
