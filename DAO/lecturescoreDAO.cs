using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
    public class lecturescoreDAO
    {
        private static lecturescoreDAO instance;

        public static lecturescoreDAO Instance
        {
            get
            {
                if (instance == null) instance = new lecturescoreDAO();
                return lecturescoreDAO.instance;
            }
            private set
            {
                lecturescoreDAO.instance = value;
            }
        }

        private lecturescoreDAO() { }

        public List<lecturescore> LoadLectureScore(string id)
        {
            string query = "SELECT * FROM GetListClass( :id )";
            DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
            List<lecturescore> scores = new List<lecturescore>();

            foreach (DataRow row in data.Rows)
            {
                lecturescore score = new lecturescore(row);
                scores.Add(score);
            }

            return scores;
        }
        public int UpdateScore(string s1, string s2, string s3, string s4, string s5, string s6)
        {
            string query = "SELECT UpdateScore( :s1 :s2 :s3 :s4 :s5 :s6 )";
            int data = DataProvider.Instance.ExcuteNonQuery(query, new object[] { s1, s2, s3, s4, s5, s6 });
            return data;
        }
    }
}
