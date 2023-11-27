using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class RegisteredCourseListDAO
	{
		private static RegisteredCourseListDAO instance;

		public static RegisteredCourseListDAO Instance
		{
			get
			{
				if (instance == null) instance = new RegisteredCourseListDAO();
				return RegisteredCourseListDAO.instance;
			}
			private set
			{
				RegisteredCourseListDAO.instance = value;
			}
		}

		private RegisteredCourseListDAO() { }

		public List<RegisteredCourseList> LoadRegisteredCourseList(string id)
		{
			string query = "SELECT * FROM GetListRegisteredByID( :id )";
			DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
			List<RegisteredCourseList> registered = new List<RegisteredCourseList>();

			foreach (DataRow row in data.Rows)
			{
				RegisteredCourseList course = new RegisteredCourseList(row);
				registered.Add(course);
			}

			return registered;
		}
	}
}
