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
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			LoadAccount();
		}

		private void LoadAccount()
		{
			string query = "SELECT * FROM account;";
			DataProvider source = new DataProvider();
			DataTable account = source.ExcuteQuery(query);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			tb_password.PasswordChar = '\u25CF';
		}

		private void btn_exit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btn_login_Click(object sender, EventArgs e)
		{
			string userName = tb_userName.Text;
			string password = tb_password.Text;
		}
	}
}
