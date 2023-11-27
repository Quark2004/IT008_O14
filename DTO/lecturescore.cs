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
        private double processScore;
        private double midtermScore;
        private double practiceScore;
        private double finalScore;
        private double courseScore;

        public string StudentId { get => studentId; set => studentId = value; }
        public string StudentName { get => studentName; set => studentName = value; }
        //public int NumberOfCredits { get => numberOfCredits; set => numberOfCredits = value; }
        public double ProcessScore { get => processScore; set => processScore = value; }
        public double MidtermScore { get => midtermScore; set => midtermScore = value; }
        public double PracticeScore { get => practiceScore; set => practiceScore = value; }
        public double FinalScore { get => finalScore; set => finalScore = value; }
        public double CourseScore { get => courseScore; set => courseScore = value; }

        public lecturescore(DataRow row)
        {
            this.StudentId = row["MSSV"].ToString();
            this.StudentName = row["Tên sinh viên"].ToString();
            //this.NumberOfCredits = int.Parse(row["Tín chỉ"].ToString());
            this.ProcessScore = double.Parse(row["Điểm quá trình"].ToString());
            this.MidtermScore = double.Parse(row["Điểm giữa kì"].ToString());
            this.PracticeScore = double.Parse(row["Điểm thực hành"].ToString());
            this.FinalScore = double.Parse(row["Điểm cuối kì"].ToString());
            this.CourseScore = double.Parse(row["Điểm học phần"].ToString());
        }
    }
}
