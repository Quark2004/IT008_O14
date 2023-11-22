using Quantrivienv._1.DAO;
using Quantrivienv._1.DTO;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quantrivienv._1
{
    public partial class fAdmin : Form
    {
        private AccountManagementDAO accountDAO;
        public fAdmin()
        {
            InitializeComponent();
            accountDAO = new AccountManagementDAO();
        }
        private void LoadData()
        {
            try
            {
                

                DataTable data = AccountManagementDAO.Instance.GetAccountData(); 

                //Dat du lieu lay tu cco so du lieu vao Datagridview
                dataGridView1.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            string username = txb_username.Text;
            string password = txb_password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            AccountManagementDTO newAccount = new AccountManagementDTO
            {
                Username = username,
                Password = password
            };

            bool result = AccountManagementDAO.Instance.AddAccount(newAccount);
            LoadData();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            string username = txb_username.Text;
            string newPassword = txb_password.Text; // mat khau moi

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Tao mot doi tuong AccountManagementDTO
            AccountManagementDTO account = new AccountManagementDTO
            {
                Username = username,
                Password = newPassword
            };

            // goi phuong thuc UpdateAccount trong DAO
            bool result = AccountManagementDAO.Instance.UpdateAccount(account);

            if (result)
            {
                MessageBox.Show("Cập nhật thông tin tài khoản thành công!");
                // cap nhat lai giao dien hoac thuc hien thao tac khac sau khi thanh cong
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin tài khoản thất bại!");
            }
            LoadData();

        }

        private void btn_Connectdata_Click(object sender, EventArgs e)
        {
            try
            {
                DataProvider.Instance.connectionStr = "Server=localhost;Port=5433;User Id=postgres;Password=1111;Database=StudentManagement;";

                string query = "SELECT * FROM Account";

                var dataTable = DataProvider.Instance.ExcuteQuery(query);

                // Hiển thị dữ liệu lên DataGridView
                if (dataTable != null)
                {
                    // Gán dữ liệu từ DataTable vào DataGridView
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string username=txb_username.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng cần tìm!");
                return;
            }

            AccountManagementDTO account = AccountManagementDAO.Instance.SearchAccount(username);

            if (account != null)
            {
                // Hien thi thong tin tai khoan
                txb_username.Text = account.Username;
                txb_password.Text = account.Password;
                // Hien thi cac thong tin khac neu can
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // lay gia tri tu cac cot cua dong da chon
                string username = row.Cells["username"].Value.ToString();
                string password = row.Cells["password"].Value.ToString();

                // Gan gia tri vao cac Textbox 
                txb_username.Text = username;
                txb_password.Text = password;
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string usernameToDelete = txb_username.Text;

            if (string.IsNullOrEmpty(usernameToDelete))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản cần xóa!");
                return;
            }

            // Gọi hàm xóa từ DAO
            bool isDeleted = AccountManagementDAO.Instance.DeleteAccount(usernameToDelete);

            if (isDeleted)
            {
                MessageBox.Show("Xóa tài khoản thành công!");
                
                LoadData(); 
            }
            else
            {
                MessageBox.Show("Xóa tài khoản không thành công!");
            }
        }

        private void txb_search_TextChanged(object sender, EventArgs e)
        {
            string username = txb_search.Text.Trim(); 

            DataView dv = new DataView(AccountManagementDAO.Instance.GetAccountData());
            dv.RowFilter = string.Format("Username LIKE '%{0}%'", username);

            dataGridView1.DataSource = dv; 
        }
    }
}

