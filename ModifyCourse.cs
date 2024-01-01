using QLSV.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV {
	public partial class ModifyCourse : Form {
		public ModifyCourse(DataGridViewRow row) {
			InitializeComponent();
			Row = row;
			LoadCourse();
		}

		public DataGridViewRow Row { get; set; }

		void LoadCourse() {
			tb_courseId.Text = Row.Cells[0].Value.ToString();
			tb_courseName.Text = Row.Cells[1].Value.ToString();
			tb_lecturerId.Text = Row.Cells[2].Value.ToString();
			tb_lecturerName.Text = Row.Cells[3].Value.ToString();
			tb_numberOfCredits.Text = Row.Cells[4].Value.ToString();
			cbb_day.Text = Row.Cells[5].Value.ToString();
			tb_period.Text = Row.Cells[6].Value.ToString();
			tb_room.Text = Row.Cells[7].Value.ToString();
			cbb_semester.Text = Row.Cells[8].Value.ToString();
			tb_schoolYear.Text = Row.Cells[9].Value.ToString();
			dtp_startDate.Text = Row.Cells[10].Value.ToString();
			dtp_endDate.Text = Row.Cells[11].Value.ToString();
		}

		private void btn_update_Click(object sender, EventArgs e) {
			string query = "SELECT updateRegisterCourse( :courseId , :lecturerId , :numberOfCredits , :day , :period , :room , :semester , :schoolYear , :startDate , :endDate )";
			bool res = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] { tb_courseId.Text, tb_lecturerId.Text, int.Parse(tb_numberOfCredits.Text), cbb_day.Text, tb_period.Text, tb_room.Text, cbb_semester.Text, tb_schoolYear.Text, dtp_startDate.Value.Date, dtp_endDate.Value.Date });
			if (res) {
				MessageBox.Show("Chỉnh sửa học phần thành công", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			} else {
				MessageBox.Show("Thay đổi không hợp lệ, vui lòng kiểm tra lại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
