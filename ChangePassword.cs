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
	public partial class ChangePassword : Form {
		public string ID { get; set; }
		public string Username { get; set; }
		public ChangePassword(string id) {
			InitializeComponent();
			ID = id;
			tb_confirmPass.PasswordChar = '\u25CF';
			tb_oldPass.PasswordChar = '\u25CF';
			tb_newPass.PasswordChar = '\u25CF';
			string getAccountQuery = "SELECT * FROM getListAccounts()";
			DataTable accounts = DataProvider.Instance.ExcuteQuery(getAccountQuery);
			
			foreach (DataRow row in accounts.Rows) {
				if (row[1].ToString() == ID) {
					Username = row[0].ToString();
				}
			}
		}

		private void btn_cancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void btn_changePass_Click(object sender, EventArgs e) {
			string oldPass = tb_oldPass.Text;
			string newPass = tb_newPass.Text;
			string confirmPass = tb_confirmPass.Text;

			if (oldPass == "" || newPass == "" || confirmPass == "") {
				MessageBox.Show("Vui lòng nhập đủ thông tin", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!CheckAccount(Username, oldPass)) {
				MessageBox.Show("Mật khẩu cũ không đúng", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (confirmPass != newPass) {
				MessageBox.Show("Mật khẩu xác nhận không trùng với mật khẩu mới", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string hashedPass = HashedPassword(newPass);

			string updatePassQuery = "SELECT UpdatePass( :username , :password )";
			bool res = (bool)DataProvider.Instance.ExcuteScalar(updatePassQuery, new object[] {Username, hashedPass});

			if (res) {
				MessageBox.Show("Thay đổi mật khẩu thành công", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
			} else {
				MessageBox.Show("Cập nhật mật khẩu thất bại!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		public bool CheckAccount(string userName, string password) {
			return GetAccount(userName, password).Rows.Count > 0;
		}

		public DataTable GetAccount(string userName, string password) {
			string query = "SELECT * FROM \"Login\"( :userName )";

			DataTable res = DataProvider.Instance.ExcuteQuery(query, new object[] { userName });

			if (res.Rows.Count > 0) {
				string hashedPassword = res.Rows[0]["password"].ToString();

				if (!VerifyPassword(password, hashedPassword)) {
					res.Rows.Clear();
				}

			}

			return res;
		}

		string HashedPassword(string password) {
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
		}

		bool VerifyPassword(string password, string hashedPassword) {
			return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
		}
	}
}
