using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    public class lecturecourse
    {
        private string courseName;
        private string courseId;
        private string day;
        private string period;
        private string SLSV;
        private string note;
   

        public string CourseName { get => courseName; set => courseName = value; }
        public string CourseId { get => courseId; set => courseId = value; }
        public string Day { get => day; set => day = value; }
        public string Period { get => period; set => period = value; }
        public string Slsv { get => SLSV; set => SLSV = value; }
        public string Note { get => note; set => note = value; }

        public lecturecourse(DataRow row)
        {
            this.CourseName = row["Tên môn học"].ToString();
            this.CourseId = row["Mã lớp"].ToString();
            this.Day = row["Thứ"].ToString();
            this.Period = row["Tiết"].ToString();
            this.Slsv = row["SLSV"].ToString();
            this.Note = row["Ghi chú"].ToString();
        }
    }
}
