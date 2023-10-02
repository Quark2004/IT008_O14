using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Npgsql;
using QLSV.DAO;

namespace QLSV
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			tb_password.PasswordChar = '\u25CF';
		}

		private void btn_exit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btn_login_Click(object sender, EventArgs e)
		{
			string userName = tb_userName.Text;
			string password = tb_password.Text;
			if (Login(userName, password)) 
			{
				this.Hide();
				switch (GetRole(userName, password))
				{
					case 1:
						StudentForm student = new StudentForm();
						student.ShowDialog();
						break;
					case 2:
						LecturerForm lecturer = new LecturerForm();
						lecturer.ShowDialog();
						break;
					case 3:
						ManagerForm manager = new ManagerForm();
						manager.ShowDialog();
						break;
				}
				this.Show();
			}
			else
			{
				MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
			}
		}
		private bool Login(string userName, string password)
		{
			return AccountDAO.Instance.Login(userName, password);
		}
		private int GetRole(string userName, string password)
		{
			return AccountDAO.Instance.GetRole(userName, password);
		}
	}
}
