using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
	public class StudentCourseRegistration
	{
		private string courseName;
		private string courseId;
		private string lecturerName;
		private int numberOfCredits;
		private string day;
		private string period;
		private string classRoom;
		private string semester;
		private string schoolYear;
		private DateTime startDate;
		private DateTime endDate;

		public string CourseName { get => courseName; set => courseName = value; }
		public string CourseId { get => courseId; set => courseId = value; }
		public string LecturerName { get => lecturerName; set => lecturerName = value; }
		public int NumberOfCredits { get => numberOfCredits; set => numberOfCredits = value; }
		public string Day { get => day; set => day = value; }
		public string Period { get => period; set => period = value; }
		public string ClassRoom { get => classRoom; set => classRoom = value; }
		public string Semester { get => semester; set => semester = value; }
		public string SchoolYear { get => schoolYear; set => schoolYear = value; }
		public DateTime StartDate { get => startDate; set => startDate = value; }
		public DateTime EndDate { get => endDate; set => endDate = value; }

		public StudentCourseRegistration(DataRow row)
		{
			this.CourseName = row["Tên môn học"].ToString();
			this.CourseId = row["Mã lớp"].ToString();
			this.LecturerName = row["Tên giảng viên"].ToString();
			this.NumberOfCredits = int.Parse(row["Số tín chỉ"].ToString());
			this.Day = row["Thứ"].ToString();
			this.Period = row["Tiết"].ToString();
			this.ClassRoom = row["Phòng"].ToString();
			this.Semester = row["Học kì"].ToString();
			this.SchoolYear = row["Năm học"].ToString();
			if (!Convert.IsDBNull(row["Ngày bắt đầu"]))
			{
				this.StartDate = Convert.ToDateTime(row["Ngày bắt đầu"]);
			}
			if (!Convert.IsDBNull(row["Ngày kết thúc"]))
			{
				this.EndDate = Convert.ToDateTime(row["Ngày kết thúc"]);
			}
		}
	}
}
