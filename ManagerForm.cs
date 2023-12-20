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
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Diagnostics;
using static iText.Signatures.LtvVerification;

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
            cbRole.Items.Add("teacher");
            cbRole.Items.Add("student");
            cbGender.Items.Add("Nam");
            cbGender.Items.Add("Nữ");
            cbGender.Items.Add("Khác");
            cbLevel.Items.Add("Sơ cấp I");
            cbLevel.Items.Add("Sơ cấp II");
            cbLevel.Items.Add("Sơ cấp II");
            cbLevel.Items.Add("Sơ cấp III");
            cbLevel.Items.Add("Trung cấp");
            cbLevel.Items.Add("Cao đẳng");
            cbLevel.Items.Add("Đại học");
            cbLevel.Items.Add("Thạc sĩ");
            cbLevel.Items.Add("Tiến sĩ");
            cbTrainingSystem.Items.Add("Chính quy");
            cbTrainingSystem.Items.Add("Không chính quy");
            
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

        //private void UpdateStatistics()
        //{
        //    try
        //    {
        //        DataTable data = AdminCourseDAO.Instance.GetCourseData();

        //        // Thực hiện thống kê dựa trên dữ liệu khóa học
        //        int totalCourses = data.Rows.Count;

        //        double averageCredits = 0;
        //        if (totalCourses > 0)
        //        {
        //            double totalCredits = data.AsEnumerable().Sum(row => row.Field<int>("numberofcredits"));
        //            averageCredits = totalCredits / totalCourses;
        //        }

        //        // Tạo DataTable mới để lưu trữ thống kê
        //        DataTable statisticsTable = new DataTable();
        //        statisticsTable.Columns.Add("Statistic", typeof(string));
        //        statisticsTable.Columns.Add("Value", typeof(string));

        //        // Thêm các dòng cho từng thống kê
        //        statisticsTable.Rows.Add("Total Courses", totalCourses.ToString());
        //        statisticsTable.Rows.Add("Average Credits", averageCredits.ToString("F2"));

        //        // Bạn có thể thêm các dòng khác cho thống kê bổ sung

        //        // Gán DataTable cho dataGridView3
        //        dataGridView3.DataSource = statisticsTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi khi cập nhật thống kê: " + ex.Message);
        //    }
        //}

        public string ID { get; set; }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (cbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn vai trò!");
                return;
            }
            string username = txb_username.Text;
            string password = txb_password.Text;
            string role = cbRole.SelectedItem.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            AccountManagementDTO newAccount = new AccountManagementDTO
            {
                Username = username,
                Password = password,
                Role = role
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
            if (cbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn vai trò!");
                return;
            }
            string username = txb_username.Text;
            string newPassword = txb_password.Text; // mat khau moi
            string role = cbRole.SelectedItem.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Tao mot doi tuong AccountManagementDTO
            AccountManagementDTO account = new AccountManagementDTO
            {
                Username = username,
                Password = newPassword,
                Role = role
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
                var dataTable = AccountManagementDAO.Instance.GetAccountsExceptAdmin();

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

            AccountManagementDTO account = AccountManagementDAO.Instance.SearchAccountExceptAdmin(username);

            if (account != null)
            {
                // Hien thi thong tin tai khoan
                txb_username.Text = account.Username;
                txb_password.Text = account.Password;
                cbRole.SelectedItem = account.Role;
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
                string role = row.Cells["role"].Value.ToString();
                // Gan gia tri vao cac Textbox 
                txb_username.Text = username;
                txb_password.Text = password;
                cbRole.SelectedItem = role;
            }
        }

        private void txb_search_TextChanged(object sender, EventArgs e)
        {
            string username = txb_search.Text.Trim();

            DataView dv = new DataView(AccountManagementDAO.Instance.GetAccountsExceptAdmin());
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
            if(dataGridView2.SelectedRows.Count == 0)
            {
                return;
            }
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                string courseId = row.Cells[0].Value.ToString();
                string courseName = row.Cells[1].Value.ToString();
                string numberOfCredits = row.Cells[2].Value.ToString();
                string schoolDay = row.Cells[3].Value.ToString();
                string lesson = row.Cells[4].Value.ToString();
                string classroom = row.Cells[5].Value.ToString();
                string semester = row.Cells[6].Value.ToString();
                string schoolYear = row.Cells[7].Value.ToString();
                string startDay = row.Cells[8].Value.ToString();
                string endDay = row.Cells[9].Value.ToString();


                txb_id.Text = courseId;
                txb_name.Text = courseName;
                txb_nberofcredits.Text = numberOfCredits;
                txb_schoolday.Text = schoolDay;
                txb_lesson.Text = lesson;
                txb_classroom.Text = classroom;
                txb_semester.Text = semester;
                txb_syear.Text = schoolYear;
                if (DateTime.TryParse(startDay, out DateTime parsedStartDay))
                {
                    dtpstartday.Value = parsedStartDay;
                }
               

                if (DateTime.TryParse(endDay, out DateTime parsedEndDay))
                {
                    dtpendday.Value = parsedEndDay;
                }
               
            }
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                
                var dataTable = AdminCourseDAO.Instance.GetCourseData();

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

            
            CourseDTO foundCourseInDatabase = AdminCourseDAO.Instance.SearchCourse(courseId, courseName);
            string filePath = "C:\\Users\\YA DAT\\Documents";

            CourseDTO foundCourseInExcel = AdminCourseDAO.Instance.SearchCourseInExcel(filePath, courseId, courseName);

            CourseDTO foundCourse = foundCourseInDatabase ?? foundCourseInExcel;

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
            //try
            //{
            //    string filterText = txb_find.Text.Trim();

            //    DataView dvDatabase = new DataView(AdminCourseDAO.Instance.GetCourseData());
            //    DataView dvExcel = new DataView(AdminCourseDAO.Instance.GetDataFromExcel("C:\\Users\\YA DAT\\Documents"));

            //    dvDatabase.RowFilter = string.Format("Id LIKE '%{0}%' OR Name LIKE '%{0}%'", filterText);
            //    dvExcel.RowFilter = string.Format("Id LIKE '%{0}%' OR Name LIKE '%{0}%'", filterText);

            //    DataTable combinedDataTable = dvDatabase.ToTable().Copy();
            //    combinedDataTable.Merge(dvExcel.ToTable());

            //    dataGridView2.DataSource = combinedDataTable;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}
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

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    UpdateStatistics();
        //}


        private void button3_Click(object sender, EventArgs e)
        {


        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void SetColumnHeaders(string columnName, string headerText)
        {
            if (dataGridView2.Columns.Contains(columnName))
            {
                dataGridView2.Columns[columnName].HeaderText = headerText;
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetColumnHeaders("id", "Mã môn học");
            SetColumnHeaders("name", "Tên môn học");
            SetColumnHeaders("numberofcredits", "Số tín chỉ");
            SetColumnHeaders("schoolday", "Ngày học");
            SetColumnHeaders("lesson", "Tiết học");
            SetColumnHeaders("classroom", "Phòng học");
            SetColumnHeaders("semester", "Học kỳ");
            SetColumnHeaders("schoolyear", "Năm học");
            SetColumnHeaders("startday", "Ngày bắt đầu");
            SetColumnHeaders("endday", "Ngày kết thúc");
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Hoặc LicenseContext.Commercial nếu bạn đang sử dụng phiên bản thương mại

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                openFileDialog.Title = "Import Excel File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    FileInfo file = new FileInfo(filePath);

                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Lấy worksheet đầu tiên, có thể thay đổi nếu cần

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        DataTable dt = new DataTable();

                        // Đọc dữ liệu từ Excel vào DataTable
                        for (int col = 1; col <= colCount; col++)
                        {
                            dt.Columns.Add(worksheet.Cells[1, col].Value.ToString());
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            DataRow excelRow = dt.NewRow();
                            for (int col = 1; col <= colCount; col++)
                            {
                                excelRow[col - 1] = worksheet.Cells[row, col].Value;
                            }
                            dt.Rows.Add(excelRow);
                        }
                        dataGridView2.DataSource = null;
                        // Hiển thị dữ liệu trong DataGridView
                        dataGridView2.DataSource = dt;
                    }

                    MessageBox.Show("Import thành công!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                MessageBox.Show("Đã xảy ra lỗi khi nhập từ Excel: " + ex.Message);
            }

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearchStudent_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filterText = txtSearchStudent.Text.Trim();

                DataView dv = new DataView(StudentInfoDAO.Instance.GetStudents());

                // Use OR condition to filter based on both ID and Name
                dv.RowFilter = string.Format("name LIKE '%{0}%'", filterText);

                gridViewStudent.DataSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCloseStudent_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSearchStudent_Click(object sender, EventArgs e)
        {
            string studentName = txtStudentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(studentName))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên cần tìm!");
                return;
            }


            StudentInfo student = StudentInfoDAO.Instance.SearchStudent(studentName);

            if (student != null)
            {
                txtStudentId.Text = student.Id;
                txtStudentName.Text = student.Name;
                birthdayDatePicker.Value = student.Birthday;
                cbGender.SelectedItem = student.Gender;
                cbLevel.SelectedItem = student.EducationLevel;
                cbTrainingSystem.SelectedItem = student.TrainingSystem;
                
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên!");
            }
        }

        private void gridViewStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtAvatarLocation.Text = "";
                DataGridViewRow row = gridViewStudent.Rows[e.RowIndex];

                string studentId = row.Cells["id"].Value.ToString();
                var student = StudentInfoDAO.Instance.GetStudentById(studentId);
                if(student != null)
                {
                    txtStudentId.Text = studentId;
                    txtStudentName.Text = student.Name;
                    cbLevel.SelectedItem = student.EducationLevel;
                    cbTrainingSystem.SelectedItem = student.TrainingSystem;
                    cbGender.SelectedItem = student.Gender;
                    birthdayDatePicker.Value = student.Birthday;
                    avatarPictureBox.Image = ConvertBytesToBitmap(student.Avatar);
                }
            }
        }

        public Bitmap ConvertBytesToBitmap(byte[] bytes)
        {
            try
            {
                if (bytes == null || bytes.Length == 0)
                {
                    // Handle invalid or empty byte array
                    return null;
                }

                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

                    // Convert Image to Bitmap (if needed)
                    return new Bitmap(image);
                }
            }
            catch (Exception ex)
            {
                // Log or display details about the exception
                Trace.WriteLine("Error creating Bitmap: " + ex.Message);
                return null;
            }
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentId.Text.Trim();
            string name = txtStudentName.Text.Trim();
            if(string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Chưa nhập mã sinh viên!");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Chưa nhập tên sinh viên!");
                return;
            }
            if (cbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn giới tính!");
                return;
            }
            if (cbTrainingSystem.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn hệ đào tạo!");
                return;
            }
            if (cbLevel.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn bậc đào tạo!");
                return;
            }
            string gender = cbGender.SelectedItem.ToString();
            string trainingSystem = cbTrainingSystem.SelectedItem.ToString();
            string level = cbLevel.SelectedItem.ToString();
            DateTime birthDay = birthdayDatePicker.Value;
            string avatarLocation = txtAvatarLocation.Text;
            byte[] avatar;
            if(!string.IsNullOrEmpty(avatarLocation))
            {
                var originalImage = new Bitmap(avatarLocation);
                avatar = ConvertBitmapToBytes(originalImage);
            } else
            {
                avatar = new byte[10];
            }
            bool res = StudentInfoDAO.Instance.AddStudent(new StudentInfo()
            {
                Id = studentId,
                Name = name,
                EducationLevel = level,
                TrainingSystem = trainingSystem,
                Gender = gender,
                Birthday = birthDay.Date,
                Avatar = avatar
            });

            if(!res)
            {
                MessageBox.Show("Thêm sinh viên thất bại!");
                return;
            }
            MessageBox.Show("Thêm sinh viên thành công!");
            btnStudentList_Click(null, null);
        }

        public byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Save the bitmap to the stream in a specific format (e.g., PNG)
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                // Get the byte array from the stream
                return stream.ToArray();
            }
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentId.Text.Trim();
            string name = txtStudentName.Text.Trim();
            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Chưa nhập mã sinh viên!");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Chưa nhập tên sinh viên!");
                return;
            }
            if (cbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn giới tính!");
                return;
            }
            if (cbTrainingSystem.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn hệ đào tạo!");
                return;
            }
            if (cbLevel.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn bậc đào tạo!");
                return;
            }
            string gender = cbGender.SelectedItem.ToString();
            string trainingSystem = cbTrainingSystem.SelectedItem.ToString();
            string level = cbLevel.SelectedItem.ToString();
            DateTime birthDay = birthdayDatePicker.Value;
            string avatarLocation = txtAvatarLocation.Text;
            var existStudent = StudentInfoDAO.Instance.GetStudentById(studentId);
            if(existStudent == null) {
                MessageBox.Show("Không tìm thấy sinh viên!");
                return;
            }
            byte[] avatar;
            if (!string.IsNullOrEmpty(avatarLocation))
            {
                var originalImage = new Bitmap(avatarLocation);
                avatar = ConvertBitmapToBytes(originalImage);
                existStudent.Avatar = avatar;
            }
            existStudent.Name = name;
            existStudent.Birthday = birthDay.Date;
            existStudent.Gender = gender;
            existStudent.EducationLevel = level;
            existStudent.TrainingSystem = trainingSystem;
            bool res = StudentInfoDAO.Instance.UpdateStudent(existStudent);

            if (!res)
            {
                MessageBox.Show("Sửa sinh viên thất bại!");
                return;
            }
            MessageBox.Show("Sửa sinh viên thành công!");
            btnStudentList_Click(null, null);
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            string id = txtStudentId.Text.Trim();
            if(string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Chưa chọn sinh viên!");
                return;
            }
            bool res = StudentInfoDAO.Instance.DeleteStudent(id);

            if (!res)
            {
                MessageBox.Show("Xoá sinh viên thất bại!");
                return;
            }
            MessageBox.Show("Xoá sinh viên thành công!");
            btnStudentList_Click(null, null);
        }

        private void btnStudentList_Click(object sender, EventArgs e)
        {
            try
            {

                var dataTable = StudentInfoDAO.Instance.GetStudents();

                // Hiển thị dữ liệu lên DataGridView
                if (dataTable != null)
                {
                    // Gán dữ liệu từ DataTable vào DataGridView
                    gridViewStudent.DataSource = dataTable;
                    gridViewStudent.Columns[gridViewStudent.ColumnCount - 1].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void gridViewStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            studentAvatarOpenFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All Files|*.*";

            if (studentAvatarOpenFile.ShowDialog() == DialogResult.OK)
            {
                var originalImage = new Bitmap(studentAvatarOpenFile.FileName);
                avatarPictureBox.Image = originalImage;
                txtAvatarLocation.Text = studentAvatarOpenFile.FileName;
            }
        }
    }
}
