using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class StudentScheduleDAO
	{
		private static StudentScheduleDAO instance;

		public static StudentScheduleDAO Instance
		{
			get
			{
				if (instance == null) instance = new StudentScheduleDAO();
				return StudentScheduleDAO.instance;
			}
			private set
			{
				StudentScheduleDAO.instance = value;
			}
		}

		private StudentScheduleDAO() { }

		public List<StudentSchedule> LoadStudentSchedule(string id)
		{
			string query = "SELECT * FROM GetScheduleById( :id )";
			DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
			List<StudentSchedule> schedules = new List<StudentSchedule>();

			foreach(DataRow row in data.Rows)
			{
				StudentSchedule schedule = new StudentSchedule(row);
				schedules.Add(schedule);
			}

			return schedules;
		}
	}
}
