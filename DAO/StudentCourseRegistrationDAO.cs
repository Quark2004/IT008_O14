using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
	public class StudentCourseRegistrationDAO
	{
		private static StudentCourseRegistrationDAO instance;

		public static StudentCourseRegistrationDAO Instance
		{
			get
			{
				if (instance == null) instance = new StudentCourseRegistrationDAO();
				return StudentCourseRegistrationDAO.instance;
			}
			private set
			{
				StudentCourseRegistrationDAO.instance = value;
			}
		}

		private StudentCourseRegistrationDAO() { }

		public List<StudentCourseRegistration> LoadStudentCourseRegistration(string id)
		{
			string query = "SELECT * FROM GetUnregisteredListById( :id )";
			DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] {id});
			List<StudentCourseRegistration> courses = new List<StudentCourseRegistration>();

			foreach (DataRow row in data.Rows)
			{
				StudentCourseRegistration course = new StudentCourseRegistration(row);
				courses.Add(course);
			}

			return courses;
		}

		public List<StudentCourseRegistration> LoadStudentCourseRegistration() {
			string query = "SELECT * FROM GetListRegisterCourse()";
			DataTable data = DataProvider.Instance.ExcuteQuery(query);
			List<StudentCourseRegistration> courses = new List<StudentCourseRegistration>();

			foreach (DataRow row in data.Rows) {
				StudentCourseRegistration course = new StudentCourseRegistration(row);
				courses.Add(course);
			}

			return courses;
		}
	}
}
