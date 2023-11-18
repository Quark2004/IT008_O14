using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLSV
{
    public partial class StudentForm : Form
    {
		DataTable dt = new DataTable();
		DataTable dtInfo = new DataTable();
		public StudentForm(string id)
        {
            InitializeComponent();
            ID = id;
            LoadInfo();
            LoadTKB();
			LoadScore();
			LoadCourseRegistration();
			LoadCourseRegistrationInfo();
			lv_Score.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public string ID { get; set; }

        #region Method
        void LoadInfo()
        {
            StudentInfo info = StudentInfoDAO.Instance.LoadStudentInfo(ID);
            lb_ID.Text = info.Id;
            lb_Name.Text = info.Name;
            lb_Birthday.Text = info.Birthday != new DateTime() ? info.Birthday.ToShortDateString() : "";
            lb_Gender.Text = info.Gender;
            lb_educationLevel.Text = info.EducationLevel;
            lb_TrainingSystem.Text = info.TrainingSystem;
            byte[] avtBytes = Encoding.ASCII.GetBytes(info.Avatar);
            string avt = "avt.jpg";
            File.WriteAllBytes(avt, avtBytes);
            pictureBox1.ImageLocation = avt;
        }

		void LoadScore()
		{
			List<StudentScore> data = StudentScoreDAO.Instance.LoadStudentScore(ID);
			Dictionary<string, List<string>> scores = new Dictionary<string, List<string>>();

			foreach (StudentScore item in data)
			{
				string key = item.Semester + "|" + item.SchoolYear;
				string scoreInfo = $"{item.CourseId}|{item.CourseName}|{item.NumberOfCredits}|{item.ProcessScore}|{item.MidtermScore}|{item.PracticeScore}|{item.FinalScore}|{item.CourseScore}";
				if (scores.ContainsKey(key))
				{
					scores[key].Add(scoreInfo);
				}
				else
				{
					scores[key] = new List<string> { scoreInfo };
				}
			}

			foreach (KeyValuePair<string, List<string>> item in scores)
			{
				int totalCredits = 0;
				float totalScore = 0;
				string[] key = item.Key.Split('|').ToArray();
				ListViewItem header = new ListViewItem();
				header.SubItems.Add(key[0]);
				header.SubItems.Add(key[1]);
				lv_Score.Items.Add(header);
				List<string> values = item.Value;
				int i = 1;
				foreach (string str in values)
				{
					string[] info = str.Split('|').ToArray();
					ListViewItem listItem = new ListViewItem(i.ToString());

					for (int j = 0; j < info.Length; j++)
					{
						listItem.SubItems.Add(info[j]);
					}
					totalCredits += int.Parse(info[2]);
					totalScore += float.Parse(info[info.Length - 1]) * int.Parse(info[2]);
					lv_Score.Items.Add(listItem);
					i++;
				}
				ListViewItem general = new ListViewItem();
				general.SubItems.Add("");
				general.SubItems.Add("Trung bình học kỳ");
				general.SubItems.Add(totalCredits.ToString());
				general.SubItems.Add("");
				general.SubItems.Add("");
				general.SubItems.Add("");
				general.SubItems.Add("");
				general.SubItems.Add((totalScore / totalCredits).ToString());
				lv_Score.Items.Add(general);
			}
		}

		void LoadCourseRegistration()
		{
			List<StudentCourseRegistration> courses = StudentCourseRegistrationDAO.Instance.LoadStudentCourseRegistration();
			dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Tên môn học", typeof(string)),
						new DataColumn("Mã môn học", typeof(string)),
						new DataColumn("Tên giảng viên",typeof(string)),new DataColumn("Số tín",typeof(int)),new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Phòng",typeof(string)),new DataColumn("Học kì",typeof(string)),new DataColumn("Năm học",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)) });
			foreach (StudentCourseRegistration course in courses)
			{
				dt.Rows.Add(course.CourseName, course.CourseId, course.LecturerName, course.NumberOfCredits, course.Day, course.Period, course.ClassRoom, course.Semester, course.SchoolYear, course.StartDate, course.EndDate);
			}
			this.data_CourseRegistration.DataSource = dt;
			data_CourseRegistration.Columns[0].ReadOnly = false;
			for (int k = 1; k < data_CourseRegistration.Columns.Count; k++)
			{
				data_CourseRegistration.Columns[k].ReadOnly = true;
			}
			this.data_CourseRegistration.AllowUserToAddRows = false;
		}

		void LoadCourseRegistrationInfo()
		{
			dtInfo.Columns.AddRange(new DataColumn[11] { new DataColumn("Tên môn học", typeof(string)),
						new DataColumn("Mã môn học", typeof(string)),
						new DataColumn("Tên giảng viên",typeof(string)),new DataColumn("Số tín",typeof(int)),new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Phòng",typeof(string)),new DataColumn("Học kì",typeof(string)),new DataColumn("Năm học",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)) });
		}

		private void LoadTKB()
		{
			List<StudentSchedule> schedules = StudentScheduleDAO.Instance.LoadStudentSchedule(ID);

			int cellWidth = 125;
			int cellHeight = 45;

			string[] timeline = { "7:30 - 8:15", "8:15 - 9:00", "9:00 - 9:45", "10:00 - 10:45", "10:45 - 11:30", "13:00 - 13:45", "13:45 - 14:30", "14:30 - 15:15", "15:30 - 16:15", "16:15 - 17:00" };

			int courseIndex = 0;

			for (int col = 1; col <= 7; col++)
			{
				for (int row = 0; row <= 10; row++)
				{
					Label lb = new Label
					{
						Width = cellWidth,
						Height = cellHeight,

						BorderStyle = BorderStyle.FixedSingle,

						Margin = new Padding(0),
						TextAlign = ContentAlignment.MiddleCenter,

					};

					flpSchedule.Controls.Add(lb);


					if (row == 0 && col == 1)
					{
						lb.Text = "Thứ/Tiết";
					}
					else if (row == 0)
					{
						lb.Text = $"Thứ {col}";
					}
					else if (col == 1)
					{
						lb.Text = $"Tiết {row}\n({timeline[row - 1]})";

					}

					if (col == 1)
					{
						lb.Width = 130;
					}


					if (courseIndex < schedules.Count)
					{
						StudentSchedule schedule = schedules[courseIndex];
						int day = Convert.ToInt32(schedule.Day);
						int startPeriod = schedule.Period[0] - '0';

						if (day == col && startPeriod == row)
						{
							lb.Text = schedule.ToString();
							lb.BackColor = Color.White;

							lb.Height = cellHeight * schedule.Period.Length;
							row = startPeriod + schedule.Period.Length - 1;
							courseIndex++;
						}
					}
				}
			}
		}
		#endregion

		private void tb_Filter_TextChanged(object sender, EventArgs e)
		{
			dt.DefaultView.RowFilter = string.Format("[Tên môn học] like '%{0}%' or [Mã môn học] like '%{0}%'", tb_Filter.Text);
		}

		private void btn_Register_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Đăng ký học phần thành công");
			List<StudentCourseRegistration> coursesRegistration = new List<StudentCourseRegistration>();
			for (int i = data_CourseRegistration.Rows.Count - 1; i >= 0; i--)
			{
				DataGridViewRow row = data_CourseRegistration.Rows[i];
				if (Convert.ToBoolean(row.Cells[0].Value))
				{
					dtInfo.Rows.Add(row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value, row.Cells[4].Value, row.Cells[5].Value, row.Cells[6].Value, row.Cells[7].Value, row.Cells[8].Value, row.Cells[9].Value, row.Cells[10].Value, row.Cells[11].Value);
					row.Cells[0].Value = null;
					dt.Rows.RemoveAt(i);
				}
			}
			this.data_RegistrationInfo.DataSource = dtInfo;
			data_RegistrationInfo.Columns[0].ReadOnly = false;
			for (int k = 1; k < data_RegistrationInfo.Columns.Count; k++)
			{
				data_RegistrationInfo.Columns[k].ReadOnly = true;
			}
			this.data_RegistrationInfo.AllowUserToAddRows = false;
		}

		private void tb_Filter2_TextChanged(object sender, EventArgs e)
		{
			dtInfo.DefaultView.RowFilter = string.Format("[Tên môn học] like '%{0}%' or [Mã môn học] like '%{0}%'", tb_Filter2.Text);
		}
	}
}
