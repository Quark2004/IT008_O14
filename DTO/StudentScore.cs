using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
	public class StudentScore
	{
		private string courseId;
		private string courseName;
		private int numberOfCredits;
		private double processScore;
		private double midtermScore;
		private double practiceScore;
		private double finalScore;
		private double courseScore;
		private string semester;
		private string schoolYear;

		public string CourseId { get => courseId; set => courseId = value; }
		public string CourseName { get => courseName; set => courseName = value; }
		public int NumberOfCredits { get => numberOfCredits; set => numberOfCredits = value; }
		public double ProcessScore { get => processScore; set => processScore = value; }
		public double MidtermScore { get => midtermScore; set => midtermScore = value; }
		public double PracticeScore { get => practiceScore; set => practiceScore = value; }
		public double FinalScore { get => finalScore; set => finalScore = value; }
		public string Semester { get => semester; set => semester = value; }
		public string SchoolYear { get => schoolYear; set => schoolYear = value; }
		public double CourseScore { get => courseScore; set => courseScore = value; }

		public StudentScore(DataRow row)
		{
			this.CourseId = row["Mã học phần"].ToString();
			this.CourseName = row["Tên học phần"].ToString();
			this.NumberOfCredits = int.Parse(row["Tín chỉ"].ToString());
			this.ProcessScore = double.Parse(row["Điểm quá trình"].ToString());
			this.MidtermScore = double.Parse(row["Điểm giữa kì"].ToString());
			this.PracticeScore = double.Parse(row["Điểm thực hành"].ToString());
			this.FinalScore = double.Parse(row["Điểm cuối kì"].ToString());
			this.CourseScore = double.Parse(row["Điểm học phần"].ToString());
			this.Semester = row["Học kì"].ToString();
			this.SchoolYear = row["Năm học"].ToString();
		}
	}
}
