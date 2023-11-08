using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLSV
{
	public partial class StudentForm : Form
	{

		public StudentForm(string id)
		{
			InitializeComponent();
			ID = id;
			LoadInfo();
			data_Schedule.RowCount = 10;
			string[] schedule = { "(7:30 - 8:15)", "(8:15 - 9:00)", "(9:00 - 9:45)", "(10:00 - 10:45)", "(10:45 - 11:30)", "(13:00 - 13:45)", "(13:45 - 14:30)", "(14:30 - 15:15)", "(15:30 - 16:15)", "(16:15 - 17:00)" };
			for (int i = 1; i <= 10; i++)
			{
				DataGridViewRow row = data_Schedule.Rows[i - 1];
				row.Cells[0].Value = "Tiết " + i + "\n" + schedule[i - 1];
			}
		}

		public string ID { get; set; }

		#region Method
		void LoadInfo()
		{
			StudentInfo info = StudentInfoDAO.Instance.LoadStudentInfo(ID);
			lb_ID.Text = info.Id;
			lb_Name.Text = info.Name;
			lb_Birthday.Text = info.Birthday.ToShortDateString();
			lb_Gender.Text = info.Gender;
			lb_educationLevel.Text = info.EducationLevel;
			lb_TrainingSystem.Text = info.TrainingSystem;

			byte[] avtBytes = Encoding.ASCII.GetBytes(info.Avatar);
			string avt = "avt.jpg";
			File.WriteAllBytes(avt, avtBytes);
			pictureBox1.ImageLocation = avt;
		}
		#endregion

		private void lv_Score_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			e.Cancel = true;
			e.NewWidth = lv_Score.Columns[e.ColumnIndex].Width;
		}
	}
}
