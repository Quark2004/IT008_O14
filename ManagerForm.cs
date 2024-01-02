using OfficeOpenXml;
using PasswordGenerator;
using QLSV.DAO;
using QLSV.DTO;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QLSV
{
    public partial class ManagerForm : Form
    {
        DataTable dtCourseOfStudent;
        DataTable courses;
        DataTable studentList;

        public ManagerForm(string id)
        {
            InitializeComponent();
            LoadAccountList();
            LoadAllCourse();
            LoadStudentList();
            Loadmonhoc();
            LoadStudentRegistrationList(data_studentList.Rows[0].Cells[0].Value.ToString());
            LoadListProfileInfo();
            ID = id;
        }
        public string ID { get; set; }


        #region ĐKHP
        void LoadAllCourse()
        {
            string query = "SELECT * FROM GetListRegisterCourse()";
            courses = DataProvider.Instance.ExcuteQuery(query);
            data_allCourse.DataSource = courses;
        }

        void LoadStudentList()
        {
            string query = "SELECT * FROM getListStudents()";
            studentList = DataProvider.Instance.ExcuteQuery(query);
            data_studentList.DataSource = studentList;
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            string getPeriodQuery = "SELECT * FROM GetListRegistrationPeriod()";
            DataTable period = DataProvider.Instance.ExcuteQuery(getPeriodQuery);
            DateTime start = Convert.ToDateTime(period.Rows[0]["Bắt đầu đăng kí học phần"]);
            DateTime end = Convert.ToDateTime(period.Rows[0]["Kết thúc đăng kí học phần"]);
            if (DateTime.Now >= start && DateTime.Now <= end)
            {
                MessageBox.Show("Đang trong thời gian ĐKHP, không thể chỉnh sửa", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(openFile.FileName);
                    Worksheet worksheet = workbook.Worksheets[0];

                    DataTable dt = worksheet.ExportDataTable();

                    data_allCourse.DataSource = dt;
                    string clearCourseQuery = "SELECT FROM clearRegisterCourse()";
                    DataProvider.Instance.ExcuteScalar(clearCourseQuery);

                    foreach (DataGridViewRow row in data_allCourse.Rows)
                    {
                        string query = "SELECT insertRegisterCourse( :courseId , :courseName , :lecturerId , :lecturerName , :numberOfCredits , :day , :period , :room , :semester , :schoolYear , :startDate , :endDate )";
                        DataProvider.Instance.ExcuteScalar(query, new object[] { row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), int.Parse(row.Cells[4].Value.ToString()), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[8].Value.ToString(), row.Cells[9].Value.ToString(), Convert.ToDateTime(row.Cells[10].Value.ToString()), Convert.ToDateTime(row.Cells[11].Value.ToString()) });
                    }
                }
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel Files (*.xlsx)|*.xlsx;*.xls";
            if (data_allCourse.Rows.Count > 0)
            {
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					using (ExcelPackage pck = new ExcelPackage(new FileInfo(saveFile.FileName)))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                        for (int i = 0; i < data_allCourse.Columns.Count; i++)
                        {
                            ws.Cells[1, i + 1].Value = data_allCourse.Columns[i].HeaderText.ToUpper();
                        }

                        for (int i = 0; i < (data_allCourse.Rows.Count); i++)
                        {
                            for (int j = 0; j < data_allCourse.Columns.Count; j++)
                            {
                                if (data_allCourse.Rows[i].Cells[j].Value != null)
                                {
                                    ws.Cells[i + 2, j + 1].Value = data_allCourse.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                        }
                        pck.Save();
                    }
					try {
						using (FileStream fs = File.Open(saveFile.FileName, FileMode.Open, FileAccess.Read, FileShare.None)) {
                            MessageBox.Show("Xuất file thành công", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					} catch (IOException ex) {
                        MessageBox.Show("Không thể lưu file", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

				}
			}
        }

        private void LoadStudentRegistrationList(string id)
        {
            dtCourseOfStudent = new DataTable();
            dtCourseOfStudent.Columns.AddRange(new DataColumn[11] { new DataColumn("Tên môn học", typeof(string)),
                        new DataColumn("Mã môn học", typeof(string)),
                        new DataColumn("Tên giảng viên",typeof(string)),new DataColumn("Số tín",typeof(int)),new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Phòng",typeof(string)),new DataColumn("Học kì",typeof(string)),new DataColumn("Năm học",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)) });
            List<RegisteredCourseList> registeredCourseLists = RegisteredCourseListDAO.Instance.LoadRegisteredCourseList(id);
            foreach (var course in registeredCourseLists)
            {
                dtCourseOfStudent.Rows.Add(course.CourseName, course.CourseId, course.LecturerName, course.NumberOfCredits, course.Day, course.Period, course.ClassRoom, course.Semester, course.SchoolYear, course.StartDate, course.EndDate);
            }
            data_courseListOfStudent.DataSource = dtCourseOfStudent;
            data_courseListOfStudent.Columns[0].ReadOnly = false;
            foreach (DataGridViewRow row in data_courseListOfStudent.Rows)
            {
                row.Cells[0].Value = true;
            }
            data_courseListOfStudent.Refresh();
            for (int k = 1; k < data_courseListOfStudent.Columns.Count; k++)
            {
                data_courseListOfStudent.Columns[k].ReadOnly = true;
            }
        }

        private void btn_modify_Click(object sender, EventArgs e)
        {
            DataGridViewRow modifyRow = data_allCourse.CurrentRow;
            if (modifyRow != null)
            {
                ModifyCourse modifyCourse = new ModifyCourse(modifyRow);
                Form bg = new Form();
                using (modifyCourse)
                {
                    bg.StartPosition = FormStartPosition.Manual;
                    bg.FormBorderStyle = FormBorderStyle.None;
                    bg.BackColor = Color.Black;
                    bg.Opacity = 0.7d;
                    bg.Size = this.Size;
                    bg.Location = this.Location;
                    bg.ShowInTaskbar = false;
                    bg.Show(this);
                    modifyCourse.Owner = bg;
                    modifyCourse.ShowDialog(bg);
                    bg.Dispose();
                }
                LoadAllCourse();
            }
            else
            {
                MessageBox.Show("Không có học phần được chọn", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void data_courseListOfStudent_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in data_courseListOfStudent.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (DataGridViewCheckBoxCell)row.Cells[0];
                checkBox.Value = true;
            }
        }

        private void data_studentList_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow curRow = data_studentList.CurrentRow;
            if (curRow != null)
            {
                string studentId = curRow.Cells[0].Value.ToString();
                tb_studentId.Text = curRow.Cells[0].Value.ToString();
                tb_studentName.Text = curRow.Cells[1].Value.ToString();
                LoadStudentRegistrationList(studentId);
            }
        }

        private void btn_accept_Click(object sender, EventArgs e)
        {
            List<string> accept = new List<string>();
            List<string> reject = new List<string>();
            foreach (DataGridViewRow row in data_courseListOfStudent.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    string query = "SELECT AcceptCourse( :studentId , :courseId )";
                    bool res = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] { tb_studentId.Text, row.Cells[2].Value });
                    if (res)
                    {
                        accept.Add(row.Cells[2].Value.ToString());
                    }
                    else
                    {
                        string queryReject = "SELECT RejectCourse( :studentId , :courseId )";
                        DataProvider.Instance.ExcuteScalar(queryReject, new object[] { tb_studentId.Text, row.Cells[2].Value });
                        reject.Add(row.Cells[2].Value.ToString());
                    }
                }
                else
                {
                    string query = "SELECT RejectCourse( :studentId , :courseId )";
                    DataProvider.Instance.ExcuteScalar(query, new object[] { tb_studentId.Text, row.Cells[2].Value });
                    reject.Add(row.Cells[2].Value.ToString());
                }
            }
            Form bg = new Form();
            RegistrationResult resultWindow = new RegistrationResult(accept, reject, "accept");
            using (resultWindow)
            {
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.BackColor = Color.Black;
                bg.Opacity = 0.7d;
                bg.Size = this.Size;
                bg.Location = this.Location;
                bg.ShowInTaskbar = false;
                bg.Show(this);
                resultWindow.Owner = bg;
                resultWindow.ShowDialog(bg);
                bg.Dispose();
            }
            LoadStudentRegistrationList(data_studentList.CurrentRow.Cells[0].Value.ToString());
        }

        private void tb_findStudent_TextChanged(object sender, EventArgs e)
        {
            studentList.DefaultView.RowFilter = string.Format("[MSSV] like '%{0}%' or [Họ tên] like '%{0}%'", tb_findStudent.Text);
        }

        private void btn_openRegistration_Click(object sender, EventArgs e)
        {
            SetCourseRegistrationPeriod setPeriodWindow = new SetCourseRegistrationPeriod();
            Form bg = new Form();
            using (setPeriodWindow)
            {
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.BackColor = Color.Black;
                bg.Opacity = 0.7d;
                bg.Size = this.Size;
                bg.Location = this.Location;
                bg.ShowInTaskbar = false;
                bg.Show(this);
                setPeriodWindow.Owner = bg;
                setPeriodWindow.ShowDialog(bg);
                bg.Dispose();
            }
        }

        #endregion

        #region Tài khoản

        DataTable accountsList;
        string curUsername;
        string curIdProfile;
        string curRole;

        private void LoadAccountList()
        {
            string query = "select * from getListAccounts()";
            accountsList = DataProvider.Instance.ExcuteQuery(query);
            data_accountsList.DataSource = accountsList;

            LoadRoles();
        }

        private void LoadRoles()
        {
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            comboSource.Add("student", "Sinh viên");
            comboSource.Add("teacher", "Giảng viên");

            cb_role.DataSource = new BindingSource(comboSource, null);
            cb_role.DisplayMember = "Value";
            cb_role.ValueMember = "Key";
        }

        private void data_accountsList_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow currRow = data_accountsList.CurrentRow;
            if (currRow != null)
            {
                tb_username.Text = currRow.Cells[0].Value.ToString();
                tb_idProfile.Text = currRow.Cells[1].Value.ToString();
                cb_role.SelectedValue = currRow.Cells[2].Value.ToString();
                tb_passGenarator.Text = string.Empty;
            }
        }

        string HashedPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
        }

        private void EnableButton(bool editing = false)
        {
            btn_createAccount.Enabled = !editing;
            btn_acceptAccount.Enabled = editing;
            btn_Cancel.Enabled = editing;
            tb_username.Enabled = editing;
            tb_idProfile.Enabled = editing;
            cb_role.Enabled = editing;
        }


        private void btn_createAccount_Click(object sender, EventArgs e)
        {
            curUsername = tb_username.Text;
            curIdProfile = tb_idProfile.Text;
            curRole = cb_role.SelectedValue.ToString();

            EnableButton(true);

            tb_username.Text = "";
            tb_idProfile.Text = "";
            string newPassword = new Password(8).Next();
            tb_passGenarator.Text = newPassword;
        }

        private void btn_acceptAccount_Click(object sender, EventArgs e)
        {
            if (tb_username.Text == "") {
                MessageBox.Show("Yêu cầu nhập tên đăng nhập", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

			if (tb_idProfile.Text == "") {
				MessageBox.Show("Yêu cầu nhập MSSV/MGV", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string hashPassword = HashedPassword(tb_passGenarator.Text);

            string query = "SELECT InsertAcc( :username , :password , :role , :idProfile )";

            bool success = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] { tb_username.Text, hashPassword, cb_role.SelectedValue.ToString(), tb_idProfile.Text });

            if (success)
            {
                MessageBox.Show("Tạo mới tài khoản thành công");
                LoadAccountList();
                EnableButton(false);
                LoadListProfileInfo();
            }
            else
            {
                MessageBox.Show("Thất bại! Tên đăng nhập / MSSV / MGV đã tồn tại");
            }
        }

        #endregion

        #region Thông tin sinh viên

        DataTable profileList;

        private void LoadListProfileInfo()
        {
            string query = "SELECT * FROM GetListProfileInfo()";
            profileList = DataProvider.Instance.ExcuteQuery(query);
            data_profileList.DataSource = profileList;
        }

        private void txtinfor_TextChanged(object sender, EventArgs e)
        {
            profileList.DefaultView.RowFilter = string.Format("[MSSV/MGV] like '%{0}%' or [Tên] like '%{0}%'", txtinfor.Text);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            tb_username.Text = curUsername;
            tb_idProfile.Text = curIdProfile;
            cb_role.SelectedValue = curRole;

            EnableButton(false);
            tb_passGenarator.Text = "";
        }

       

        private void btn_editProfile_Click(object sender, EventArgs e)
        {
            DataGridViewRow modifyRow = data_profileList.CurrentRow;
            ModifyProfile modifyProfile = new ModifyProfile(modifyRow.Cells[0].Value.ToString());

            Form bg = new Form();
            using (modifyProfile)
            {
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.BackColor = Color.Black;
                bg.Opacity = 0.7d;
                bg.Size = this.Size;
                bg.Location = this.Location;
                bg.ShowInTaskbar = false;
                bg.Show(this);
                modifyProfile.Owner = bg;
                modifyProfile.ShowDialog(bg);
                bg.Dispose();
            }

            LoadListProfileInfo();
            LoadStudentList();
        }

        #endregion

        #region monhoc
        DataTable dt = new DataTable();
        DataTable profile = new DataTable();

        void Loadmonhoc()
        {
            btnadd.Enabled = false;
            btndelete.Enabled = false;
            btnok.Enabled = false;
            btnexit.Enabled = false;
            txtmssv.Enabled = false;
            dt.Columns.AddRange(new DataColumn[11] { new DataColumn("Tên môn học", typeof(string)),
                        new DataColumn("Mã môn học", typeof(string)),
                        new DataColumn("Tên giảng viên",typeof(string)),new DataColumn("Số tín",typeof(int)),new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Phòng",typeof(string)),new DataColumn("Học kì",typeof(string)),new DataColumn("Năm học",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)) });
            List<StudentCourseRegistration> courses = StudentCourseRegistrationDAO.Instance.LoadStudentCourseRegistration();
            foreach (StudentCourseRegistration course in courses)
            {
                dt.Rows.Add(course.CourseName, course.CourseId, course.LecturerName, course.NumberOfCredits, course.Day, course.Period, course.ClassRoom, course.Semester, course.SchoolYear, course.StartDate.ToString("MM/dd/yyyy"), course.EndDate.ToString("MM/dd/yyyy"));
            }
            this.dtmonhoc.DataSource = dt;
            profile.Columns.AddRange(new DataColumn[2] { new DataColumn("MSSV/MGV", typeof(string)),
                        new DataColumn("Họ tên", typeof(string)) });

        }

        //Hiển thị sv từng lớp
        void Profilecourse(string id)
        {
            profile.Rows.Clear();
            List<managercourseid> managercourseids = managercourseidDAO.Instance.Loadprofilebycourseid(courseid);

            foreach (var managercourse in managercourseids)
            {
                profile.Rows.Add(managercourse.Id, managercourse.Name);
            }

            this.dtprobycourse.DataSource = profile;
        }

        //Tìm kiếm môn học
        private void txtmonhoc_TextChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("[Tên môn học] like '%{0}%' or [Mã môn học] like '%{0}%'", txtmonhoc.Text);

        }

        //Chọn lớp để hiển thị
        private void dtmonhoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataGridViewRow row = dtmonhoc.Rows[e.RowIndex];
            courseid = row.Cells[1].Value.ToString();
            Profilecourse(courseid);
            btnadd.Enabled = true;
        }

        //tìm kiếm sv
        private void txtsv_TextChanged(object sender, EventArgs e)
        {
            profile.DefaultView.RowFilter = string.Format("[MSSV/MGV] like '%{0}%' or [Họ tên] like '%{0}%'", txtsv.Text);

        }
        string courseid;

        //Chọn sv để thao tác
        private void dtprobycourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dtprobycourse.Rows[e.RowIndex];
            string mssv = row.Cells[0].Value.ToString();
            txtmssv.Text = mssv;
            btndelete.Enabled = true;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            btndelete.Enabled = false;
            btnadd.Enabled = false;
            btnok.Enabled = true;
            btnexit.Enabled = true;
            txtmssv.Enabled = true;
            txtmssv.Text = "";
        }
        private void btndelete_Click(object sender, EventArgs e)
        {

            btndelete.Enabled = false;
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sv có mssv: " + txtmssv.Text, "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                bool kq = managercourseidDAO.Instance.deletestudent(txtmssv.Text, courseid);
                Profilecourse(courseid);
            }
            txtmssv.Text = "";
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            btnexit.Enabled = false;
            btnok.Enabled = false;
            btnadd.Enabled = true;
            txtmssv.Enabled = false;
            bool check = false;
            DataTable temp = new DataTable();
            string query = "select * from getListStudents()";
            temp = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow row in temp.Rows)
            {
                if (txtmssv.Text == row[0].ToString())
                    check = true;
                break;
            }
            if (check == false)
            {
                MessageBox.Show("MSSV không hợp lệ");
                txtmssv.Text = "";
            }
            else
            {
                bool kq = managercourseidDAO.Instance.Addstudent(txtmssv.Text, courseid);
                if (kq)
                {
                    MessageBox.Show("Đã thêm thành công", "Thêm sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Profilecourse(courseid);
                    txtmssv.Text = "";
                }
                else
                {
                    MessageBox.Show("MSSV không hợp lệ");
                    txtmssv.Text = "";
                }
            }

        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            txtmssv.Enabled = false;
            txtmssv.Text = "";
            btnexit.Enabled = false;
            btnok.Enabled = false;
            btnadd.Enabled = true;
        }
        #endregion

        private void ManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form bg = new Form();
            CloseWindow logOut = new CloseWindow();
            using (logOut)
            {
                bg.StartPosition = FormStartPosition.Manual;
                bg.FormBorderStyle = FormBorderStyle.None;
                bg.BackColor = Color.Black;
                bg.Opacity = 0.7d;
                bg.Size = this.Size;
                bg.Location = this.Location;
                bg.ShowInTaskbar = false;
                bg.Show(this);
                logOut.Owner = bg;
                logOut.ShowDialog(bg);
                bg.Dispose();
            }
            e.Cancel = logOut.IsNotClosed;
        }

       
    }
}
