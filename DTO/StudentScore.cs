using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
			this.CourseId = !DBNull.Value.Equals(row["Mã học phần"]) ? row["Mã học phần"].ToString() : "";
			this.CourseName = !DBNull.Value.Equals(row["Tên học phần"]) ? row["Tên học phần"].ToString() : "";
			this.NumberOfCredits = !DBNull.Value.Equals(row["Tín chỉ"]) ? int.Parse(row["Tín chỉ"].ToString()) : -1;
			this.ProcessScore = !DBNull.Value.Equals(row["Điểm quá trình"]) ? double.Parse(row["Điểm quá trình"].ToString()) : -1;
			this.MidtermScore = !DBNull.Value.Equals(row["Điểm giữa kì"]) ? double.Parse(row["Điểm giữa kì"].ToString()) : -1;
			this.PracticeScore = !DBNull.Value.Equals(row["Điểm thực hành"]) ? double.Parse(row["Điểm thực hành"].ToString()) : -1;
			this.FinalScore = !DBNull.Value.Equals(row["Điểm cuối kì"]) ? double.Parse(row["Điểm cuối kì"].ToString()) : -1;
			this.CourseScore = !DBNull.Value.Equals(row["Điểm học phần"]) ? double.Parse(row["Điểm học phần"].ToString()) : -1;
			this.Semester = !DBNull.Value.Equals(row["Học kì"]) ? row["Học kì"].ToString() : "";
			this.SchoolYear = !DBNull.Value.Equals(row["Năm học"]) ? row["Năm học"].ToString() : "";
		}
	}
}