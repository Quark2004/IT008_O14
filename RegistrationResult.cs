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
	public partial class RegistrationResult : Form {
		public RegistrationResult(List<string> success, List<string> error, string action) {
			InitializeComponent();
			Success = success;
			Error = error;
			Action = action;
			lb_successList.Text = "";
			lb_errorList.Text = "";
			LoadResult();
		}

		public List<string> Success { get; set; }
		public List<string> Error { get; set; }
		public string Action { get; set; }

		void LoadResult() {
			lb_successNum.Text = Success.Count + " " + lb_successNum.Text;
			lb_errorNum.Text = Error.Count + " " + lb_errorNum.Text;
			foreach (string item in Success) {
				lb_successList.Text += item + (Action != "cancel" ? ": Sinh viên đăng ký thành công\n" : ": Sinh viên hủy thành công\n");
			}
			if (Action != "cancel") {
				foreach (string item in Error) {
					lb_errorList.Text += item + ": Trùng lịch học/trùng môn\n";
				}
			} else {
				lb_errorList.Visible = false;
				lb_errorNum.Visible = false;
				pb_error.Visible = false;
			}
			pb_error.Location = new Point(pb_success.Location.X, lb_successList.Location.Y + lb_successList.Height + 50);
			lb_errorNum.Location = new Point(lb_successNum.Location.X, lb_successList.Location.Y + lb_successList.Height + 55);
			lb_errorList.Location = new Point(lb_successList.Location.X, pb_error.Location.Y + pb_error.Height + 10);
		}

		private void pb_close_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}
