using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
     public class lecturecourseDAO
     {
        private static lecturecourseDAO instance;

        public static lecturecourseDAO Instance
        {
            get
            {
                if (instance == null) instance = new lecturecourseDAO();
                return lecturecourseDAO.instance;
            }
            private set
            {
                lecturecourseDAO.instance = value;
            }
        }

        private lecturecourseDAO() { }
        public List<lecturecourse> LoadSlecturecourseDAO(string id)
        {
            string query = "SELECT * FROM GetClassInCharge( :id )";
            DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
            List<lecturecourse> courses = new List<lecturecourse>();

            foreach (DataRow row in data.Rows)
            {
                lecturecourse course = new lecturecourse(row);
                courses.Add(course);
            }

            return courses;
        }
    }
}
