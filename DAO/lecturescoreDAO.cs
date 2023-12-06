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
        public string UpdateScore(string s1, string s2, float s3, float s4, float s5, float s6)
        {
            string query = "SELECT UpdateScore( :mammon , :mssv , :diemQT , :diemGK , :diemTH , :diemCK )";
            var data = DataProvider.Instance.ExcuteQuery(query, new object[] { s1, s2, s3, s4, s5, s6 });
            return data.Rows[0][0].ToString();
        }
    }
}
