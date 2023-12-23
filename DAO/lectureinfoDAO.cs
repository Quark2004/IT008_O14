using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DAO
{
    public class lectureinfoDAO
    {
        private static lectureinfoDAO instance;

        public static lectureinfoDAO Instance
        {
            get
            {
                if (instance == null) instance = new lectureinfoDAO();
                return lectureinfoDAO.instance;
            }
            private set
            {
                lectureinfoDAO.instance = value;
            }
        }

        private lectureinfoDAO() { }

        public lectureinfo LoadLectureInfo(string id)
        {
            string query = "SELECT * FROM LoadProfileById( :id )";
            DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { id });
            lectureinfo info = new lectureinfo(data.Rows[0]);

            return info;
        }
    }
}
