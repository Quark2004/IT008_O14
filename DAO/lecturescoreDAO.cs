using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;

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

        private static object ParseFloatValue(string stringValue)
        {
            return string.IsNullOrEmpty(stringValue) ? (object)DBNull.Value : float.Parse(stringValue);
        }

        public string UpdateScore(string mammon, string mssv, string diemQT, string diemGK, string diemTH, string diemCK)
        {
            string query = "SELECT UpdateScore( :mammon , :mssv , :diemQT , :diemGK , :diemTH , :diemCK )";
            var data = DataProvider.Instance.ExcuteQuery(query, new object[] { mammon, mssv,
                ParseFloatValue(diemQT),
                ParseFloatValue(diemGK),
                ParseFloatValue(diemTH),
                ParseFloatValue(diemCK)
            });
            return data.Rows[0][0].ToString();
        }

        public List<lecturescore> LoadRatioCourse(string id)
        {
            string query = "select * from getRaitoScoreByCourseId( :id )";
            DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
            List<lecturescore> scores = new List<lecturescore>();

            foreach (DataRow row in data.Rows)
            {
                lecturescore score = new lecturescore(row);
                scores.Add(score);
            }

            return scores;
        }

        //public bool UpdateRatioScore(string mammon, float diemQT, float diemGK, float diemTH, float diemCK)
        //{
        //    string query = "SELECT UpdateRatioScore( :mamon , :diemQT , :diemGK , :diemTH , :diemCK )";
        //    //bool data = DataProvider.Instance.ExcuteQuery(query, new object[] { mammon, diemQT,diemGK,diemTH,diemCK });
        //    return data;
        //}
    }
}
