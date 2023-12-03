using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static QLSV.DAO.AccoutManagementDAO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace QLSV
{
    public partial class ManagerForm : Form
    {
        private AccountManagementDAO accountDAO;
        public ManagerForm(string id)
        {
            InitializeComponent();
            ID = id;
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
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                DataTable data = AdminCourseDAO.Instance.GetCourseData();

                // Thực hiện thống kê dựa trên dữ liệu khóa học
                int totalCourses = data.Rows.Count;

                double averageCredits = 0;
                if (totalCourses > 0)
                {
                    double totalCredits = data.AsEnumerable().Sum(row => row.Field<int>("numberofcredits"));
                    averageCredits = totalCredits / totalCourses;
                }

                // Tạo DataTable mới để lưu trữ thống kê
                DataTable statisticsTable = new DataTable();
                statisticsTable.Columns.Add("Statistic", typeof(string));
                statisticsTable.Columns.Add("Value", typeof(string));

                // Thêm các dòng cho từng thống kê
                statisticsTable.Rows.Add("Total Courses", totalCourses.ToString());
                statisticsTable.Rows.Add("Average Credits", averageCredits.ToString("F2"));

                // Bạn có thể thêm các dòng khác cho thống kê bổ sung

                // Gán DataTable cho dataGridView3
                dataGridView3.DataSource = statisticsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thống kê: " + ex.Message);
            }
        }

        public string ID { get; set; }

        private void tabPage1_Click(object sender, EventArgs e)
        {

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
            if (result)
            {
                MessageBox.Show("Thêm thông tin tài khoản thành công");
                LoadData();
            }
            else
            {
                MessageBox.Show("Thêm thông tin thất bại");
            }
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
                LoadData();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin tài khoản thất bại!");
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

        private void btn_Connectdata_Click(object sender, EventArgs e)
        {
            try
            {
                DataProvider.Instance.connectionStr = "Server=localhost;Port=5433;User Id=postgres;Password=1111;Database=StudentManagement;";

                string query = "SELECT * FROM Account where role='student' or role ='teacher'";

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

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string username = txb_username.Text;

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

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void txb_search_TextChanged(object sender, EventArgs e)
        {
            string username = txb_search.Text.Trim();

            DataView dv = new DataView(AccountManagementDAO.Instance.GetAccountData());
            dv.RowFilter = string.Format("Username LIKE '%{0}%'", username);

            dataGridView1.DataSource = dv;
        }

        private void btn_addd_Click(object sender, EventArgs e)
        {
            string courseId = txb_id.Text;
            string courseName = txb_name.Text;

            int numberOfCredits;
            bool isValidNumberOfCredits = int.TryParse(txb_nberofcredits.Text, out numberOfCredits);

            string schoolDay = txb_schoolday.Text;
            string lesson = txb_lesson.Text;
            string classroom = txb_classroom.Text;

            string coursesemester = txb_semester.Text;

            string schoolYear = txb_syear.Text;

            DateTime startDay = dtpstartday.Value;
            DateTime endDay = dtpendday.Value;


            if (!isValidNumberOfCredits)
            {

                MessageBox.Show("Vui lòng nhập đúng định dạng thông tin.");
                return;
            }


            CourseDTO newCourse = new CourseDTO
            {
                id = courseId,
                name = courseName,
                numberofcredits = numberOfCredits,
                schoolday = schoolDay,
                lesson = lesson,
                classroom = classroom,
                semester = coursesemester,
                schoolyear = schoolYear,
                startday = startDay,
                endday = endDay

            };

            bool result = AdminCourseDAO.Instance.AddCourse(newCourse);
            if (result)
            {
                MessageBox.Show("Thêm khóa học thành công!");
                LoadData1();
            }
            else
            {
                MessageBox.Show("Thêm khóa học thất bại!");
            }
        }

        private void btn_editt_Click(object sender, EventArgs e)
        {
            string courseIdToUpdate = txb_id.Text;
            string updatedCourseName = txb_name.Text;

            int updatedNumberOfCredits;
            bool isValidUpdatedNumberOfCredits = int.TryParse(txb_nberofcredits.Text, out updatedNumberOfCredits);

            string updatedSchoolDay = txb_schoolday.Text;
            string updatedLesson = txb_lesson.Text;
            string updatedClassroom = txb_classroom.Text;

            string updatedsemester = txb_semester.Text;

            string updatedSchoolYear = txb_syear.Text;

            DateTime startDay = dtpstartday.Value;
            DateTime endDay = dtpendday.Value;


            if (!isValidUpdatedNumberOfCredits)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng thông tin.");
                return;
            }


            CourseDTO updatedCourse = new CourseDTO
            {
                id = courseIdToUpdate,
                name = updatedCourseName,
                numberofcredits = updatedNumberOfCredits,
                schoolday = updatedSchoolDay,
                lesson = updatedLesson,
                classroom = updatedClassroom,
                semester = updatedsemester,
                schoolyear = updatedSchoolYear,
                startday = dtpstartday.Value,
                endday = dtpendday.Value

            };


            bool updateResult = AdminCourseDAO.Instance.UpdateCourse(updatedCourse);

            if (updateResult)
            {
                MessageBox.Show("Cập nhật thông tin khóa học thành công!");
                LoadData1();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin khóa học thất bại!");
            }
        }

        private void btn_deletee_Click(object sender, EventArgs e)
        {
            string courseIdToDelete = txb_id.Text;


            bool isDeleted = AdminCourseDAO.Instance.DeleteCourse(courseIdToDelete);
            if (isDeleted)
            {
                MessageBox.Show("Xóa khóa học thành công!");
                LoadData1();
            }
            else
            {
                MessageBox.Show("Xóa khóa học không thành công!");
            }
        }
        private void LoadData1()
        {
            try
            {


                DataTable data = AdminCourseDAO.Instance.GetCourseData();

                //Dat du lieu lay tu cco so du lieu vao Datagridview
                dataGridView2.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                string courseId = row.Cells["id"].Value.ToString();
                string courseName = row.Cells["name"].Value.ToString();
                string numberOfCredits = row.Cells["numberofcredits"].Value.ToString();
                string schoolDay = row.Cells["schoolday"].Value.ToString();
                string lesson = row.Cells["lesson"].Value.ToString();
                string classroom = row.Cells["classroom"].Value.ToString();
                string semester = row.Cells["semester"].Value.ToString();
                string schoolYear = row.Cells["schoolyear"].Value.ToString();
                string startDay = row.Cells["startday"].Value.ToString();
                string endDay = row.Cells["endday"].Value.ToString();


                txb_id.Text = courseId;
                txb_name.Text = courseName;
                txb_nberofcredits.Text = numberOfCredits;
                txb_schoolday.Text = schoolDay;
                txb_lesson.Text = lesson;
                txb_classroom.Text = classroom;
                txb_semester.Text = semester;
                txb_syear.Text = schoolYear;
                dtpstartday.Text = startDay;
                dtpendday.Text = endDay;
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                DataProvider.Instance.connectionStr = "Server=localhost;Port=5433;User Id=postgres;Password=1111;Database=StudentManagement;";

                string query = "SELECT * FROM Course ";

                var dataTable = DataProvider.Instance.ExcuteQuery(query);

                // Hiển thị dữ liệu lên DataGridView
                if (dataTable != null)
                {
                    // Gán dữ liệu từ DataTable vào DataGridView
                    dataGridView2.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_Exitt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Find_Click(object sender, EventArgs e)
        {
            string courseId = txb_id.Text.Trim();
            string courseName = txb_find.Text.Trim();

            if (string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(courseName))
            {
                MessageBox.Show("Vui lòng nhập mã khóa học cần tìm!");
                return;
            }

            CourseDTO foundCourse = AdminCourseDAO.Instance.SearchCourse(courseId, courseName);

            if (foundCourse != null)
            {
                txb_name.Text = foundCourse.name;
                txb_nberofcredits.Text = foundCourse.numberofcredits.ToString();
                txb_schoolday.Text = foundCourse.schoolday;
                txb_lesson.Text = foundCourse.lesson;
                txb_classroom.Text = foundCourse.classroom;
                txb_semester.Text = foundCourse.semester.ToString();
                txb_syear.Text = foundCourse.schoolyear;
                dtpstartday.Text = foundCourse.startday.ToString("yyyy-MM-dd");
                dtpendday.Text = foundCourse.endday.ToString("yyyy-MM-dd");
            }
            else
            {
                MessageBox.Show("Không tìm thấy khóa học!");
            }

        }

        private void txb_find_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filterText = txb_find.Text.Trim();

                DataView dv = new DataView(AdminCourseDAO.Instance.GetCourseData());

                // Use OR condition to filter based on both ID and Name
                dv.RowFilter = string.Format("Id LIKE '%{0}%' OR Name LIKE '%{0}%'", filterText);

                dataGridView2.DataSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_ExportToPdf_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the LicenseContext to NonCommercial or Commercial based on your use case
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Export to Excel";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    FileInfo file = new FileInfo(filePath);

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Student Management Data");

                        // Add headers
                        for (int i = 1; i <= dataGridView2.Columns.Count; i++)
                        {
                            worksheet.Cells[1, i].Value = dataGridView2.Columns[i - 1].HeaderText;
                            worksheet.Cells[1, i].Style.Font.Bold = true;
                        }

                        // Add data
                        for (int i = 0; i < dataGridView2.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView2.Columns.Count; j++)
                            {
                                worksheet.Cells[i + 2, j + 1].Value = dataGridView2.Rows[i].Cells[j].Value;
                            }
                        }

                        package.Save();
                    }

                    MessageBox.Show("Export to Excel successful!");
                }
            }
            catch (Exception ex)
            {
                // Print the exception details to understand the issue
                Console.WriteLine("Error exporting to Excel: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);

                // If the exception has an inner exception, print its details as well
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException: " + ex.InnerException.Message);
                    Console.WriteLine("InnerException StackTrace: " + ex.InnerException.StackTrace);
                }

                // Show a user-friendly error message
                MessageBox.Show("An error occurred while exporting to Excel. Check the console for details.");
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateStatistics();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem DataGridView có dữ liệu không
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để thống kê.");
                return;
            }

            // Khởi tạo DataTable mới để lưu thông tin thống kê
            DataTable statisticsTable = new DataTable();

            // Thêm cột "Role" và "Số Lượng" vào DataTable
            statisticsTable.Columns.Add("Role", typeof(string));
            statisticsTable.Columns.Add("Số Lượng", typeof(int));

            // Duyệt qua từng dòng trong DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Kiểm tra xem ô "role" có giá trị không
                if (row.Cells["role"].Value != null)
                {
                    // Lấy giá trị từ cột "Role"
                    string role = row.Cells["role"].Value.ToString();

                    // Tìm xem "Role" đã có trong DataTable chưa
                    DataRow existingRow = statisticsTable.AsEnumerable()
                        .FirstOrDefault(r => r.Field<string>("Role") == role);

                    // Nếu đã có, tăng giá trị "Số Lượng" lên 1
                    if (existingRow != null)
                    {
                        existingRow.SetField("Số Lượng", existingRow.Field<int>("Số Lượng") + 1);
                    }
                    // Nếu chưa có, thêm một dòng mới với "Role" và "Số Lượng" là 1
                    else
                    {
                        statisticsTable.Rows.Add(role, 1);
                    }
                }
            }


            // Hiển thị DataTable kết quả lên DataGridView4
            dataGridView4.DataSource = statisticsTable;
        }
    }
}
