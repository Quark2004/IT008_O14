using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    public class lecturescore
    {
        private string studentId;
        private string studentName;
       // private int numberOfCredits;
        private float processScore;
        private float midtermScore;
        private float practiceScore;
        private float finalScore;
        private float courseScore;

        public string StudentId { get => studentId; set => studentId = value; }
        public string StudentName { get => studentName; set => studentName = value; }
        //public int NumberOfCredits { get => numberOfCredits; set => numberOfCredits = value; }
        public float ProcessScore { get => processScore; set => processScore = value; }
        public float MidtermScore { get => midtermScore; set => midtermScore = value; }
        public float PracticeScore { get => practiceScore; set => practiceScore = value; }
        public float FinalScore { get => finalScore; set => finalScore = value; }
        public float CourseScore { get => courseScore; set => courseScore = value; }

        public lecturescore(DataRow row)
        {
            this.StudentId = row["MSSV"].ToString();
            this.StudentName = row["Tên sinh viên"].ToString();
            //this.NumberOfCredits = int.Parse(row["Tín chỉ"].ToString());
            this.ProcessScore = row["Điểm quá trình"] == System.DBNull.Value ? -1 : float.Parse(row["Điểm quá trình"].ToString());
            this.MidtermScore = row["Điểm giữa kì"] == System.DBNull.Value ? -1 : float.Parse(row["Điểm giữa kì"].ToString());
            this.PracticeScore = row["Điểm thực hành"] == System.DBNull.Value ? -1 : float.Parse(row["Điểm thực hành"].ToString());
            this.FinalScore = row["Điểm cuối kì"] == System.DBNull.Value ? -1 : float.Parse(row["Điểm cuối kì"].ToString());
            this.CourseScore = row["Điểm học phần"] == System.DBNull.Value ? -1 : float.Parse(row["Điểm học phần"].ToString());
        }
    }
}
