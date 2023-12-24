using QLSV.DAO;
using System;
using OfficeOpenXml;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iText.Layout.Element;
using Spire.Xls;
using QLSV.DTO;

namespace QLSV {
	public partial class ManagerForm : Form {
		DataTable dtCourseOfStudent;
		DataTable courses;
		DataTable studentList;

		public ManagerForm(string id) {
			InitializeComponent();
			LoadAllCourse();
			LoadStudentList();
			LoadStudentRegistrationList(data_studentList.Rows[0].Cells[0].Value.ToString());
			ID = id;
		}
		public string ID { get; set; }

		#region ĐKHP
		void LoadAllCourse() {
			string query = "SELECT * FROM GetListRegisterCourse()";
			courses = DataProvider.Instance.ExcuteQuery(query);
			data_allCourse.DataSource = courses;
		}

		void LoadStudentList() {
			string query = "SELECT * FROM getListStudents()";
			studentList = DataProvider.Instance.ExcuteQuery(query);
			data_studentList.DataSource = studentList;
		}

		private void btn_import_Click(object sender, EventArgs e) {
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
			if (openFile.ShowDialog() == DialogResult.OK) {
				Workbook workbook = new Workbook();
				workbook.LoadFromFile(openFile.FileName);
				Worksheet worksheet = workbook.Worksheets[0];

				DataTable dt = worksheet.ExportDataTable();

				data_allCourse.DataSource = dt;
			}
		}

		private void btn_export_Click(object sender, EventArgs e) {
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "Excel Files (*.xlsx)|*.xlsx;*.xls";
			if (data_allCourse.Rows.Count > 0) {
				if (saveFile.ShowDialog() == DialogResult.OK) {
					using (ExcelPackage pck = new ExcelPackage(new FileInfo(saveFile.FileName))) {
						ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
						for (int i = 0; i < data_allCourse.Columns.Count; i++) {
							ws.Cells[1, i + 1].Value = data_allCourse.Columns[i].HeaderText.ToUpper();
						}

						for (int i = 0; i < (data_allCourse.Rows.Count); i++) {
							for (int j = 0; j < data_allCourse.Columns.Count; j++) {
								if (data_allCourse.Rows[i].Cells[j].Value != null) {
									ws.Cells[i + 2, j + 1].Value = data_allCourse.Rows[i].Cells[j].Value.ToString();
								}
							}
						}
						pck.Save();
					}
				}
			}
		}

		private void LoadStudentRegistrationList(string id) {
			dtCourseOfStudent = new DataTable();
			dtCourseOfStudent.Columns.AddRange(new DataColumn[11] { new DataColumn("Tên môn học", typeof(string)),
						new DataColumn("Mã môn học", typeof(string)),
						new DataColumn("Tên giảng viên",typeof(string)),new DataColumn("Số tín",typeof(int)),new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Phòng",typeof(string)),new DataColumn("Học kì",typeof(string)),new DataColumn("Năm học",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)) });
			List<RegisteredCourseList> registeredCourseLists = RegisteredCourseListDAO.Instance.LoadRegisteredCourseList(id);
			foreach (var course in registeredCourseLists) {
				dtCourseOfStudent.Rows.Add(course.CourseName, course.CourseId, course.LecturerName, course.NumberOfCredits, course.Day, course.Period, course.ClassRoom, course.Semester, course.SchoolYear, course.StartDate, course.EndDate);
			}
			data_courseListOfStudent.DataSource = dtCourseOfStudent;
			data_courseListOfStudent.Columns[0].ReadOnly = false;
			foreach (DataGridViewRow row in data_courseListOfStudent.Rows) {
				row.Cells[0].Value = true;
			}
			data_courseListOfStudent.Refresh();
			for (int k = 1; k < data_courseListOfStudent.Columns.Count; k++) {
				data_courseListOfStudent.Columns[k].ReadOnly = true;
			}
		}

		private void btn_modify_Click(object sender, EventArgs e) {
			DataGridViewRow modifyRow = data_allCourse.CurrentRow;
			ModifyCourse modifyCourse = new ModifyCourse(modifyRow);
			modifyCourse.ShowDialog();
		}

		private void data_courseListOfStudent_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
			foreach (DataGridViewRow row in data_courseListOfStudent.Rows) {
				DataGridViewCheckBoxCell checkBox = (DataGridViewCheckBoxCell)row.Cells[0];
				checkBox.Value = true;
			}
		}

		private void data_studentList_SelectionChanged(object sender, EventArgs e) {
			DataGridViewRow curRow = data_studentList.CurrentRow;
			if (curRow != null) {
				string studentId = curRow.Cells[0].Value.ToString();
				tb_studentId.Text = curRow.Cells[0].Value.ToString();
				tb_studentName.Text = curRow.Cells[1].Value.ToString();
				LoadStudentRegistrationList(studentId);
			}
		}

		private void btn_accept_Click(object sender, EventArgs e) {
			List<string> success = new List<string>();
			List<string> error = new List<string>();
			foreach (DataGridViewRow row in data_courseListOfStudent.Rows) {
				if (Convert.ToBoolean(row.Cells[0].Value)) {
					string query = "SELECT AcceptCourse( :studentId , :courseId )";
					bool result = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] {tb_studentId.Text, row.Cells[1].Value });
					if (result) {
						success.Add(row.Cells[2].Value.ToString());
					} else {
						error.Add(row.Cells[2].Value.ToString());
					}
				} else {
					string query = "SELECT RejectCourse( :studentId , :courseId )";
					bool result = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] { tb_studentId.Text, row.Cells[1].Value });
				}
			}
			RegistrationResult resultWindow = new RegistrationResult(success, error);
			resultWindow.ShowDialog();
		}

		private void tb_findStudent_TextChanged(object sender, EventArgs e) {
			studentList.DefaultView.RowFilter = string.Format("[MSSV] like '%{0}%' or [Họ tên] like '%{0}%'", tb_findStudent.Text);
		}

		private void btn_openRegistration_Click(object sender, EventArgs e) {
			SetCourseRegistrationPeriod setPeriodWindow = new SetCourseRegistrationPeriod();
			setPeriodWindow.ShowDialog();
		}

		#endregion
	}
}
