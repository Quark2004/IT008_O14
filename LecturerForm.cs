using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Xml.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLSV
{
    public partial class LecturerForm : Form
    {
        DataTable dt = new DataTable();
        public LecturerForm(string id)
        {
            InitializeComponent();
            ID = id;
            Loadlichday();
            LoadDSLop();
            Loadinfo();
            Loadthongke();
            btnok.Enabled = false;
            btnnhap.Enabled = false;
            //LoadTKB();
        }


        public string ID { get; set; }

        void Loadinfo()
        {
            lectureinfo info = lectureinfoDAO.Instance.LoadStudentInfo(ID);
            lbid.Text = info.Id;
            lbname.Text = info.Name;
            DateTime temp = info.Birthday;
            lbbirth.Text = info.Birthday.ToString();
            lbgender.Text = info.Gender;
            lbkhoa.Text = info.EducationLevel;
            lbmakhoa.Text = info.TrainingSystem;
            byte[] avtBytes = Encoding.ASCII.GetBytes(info.Avatar);
            string avt = "avt.jpg";
            File.WriteAllBytes(avt, avtBytes);
            pictureBox1.ImageLocation = avt;
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            //enable();
        }

        #region load_tab_lichday
        void Loadlichday()
        {
            //List<lecturecourse> courses = lecturecourseDAO.Instance.LoadSlecturecourseDAO(ID);
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Mã môn học", typeof(string)),
                        new DataColumn("Tên môn học", typeof(string)),new DataColumn("Phòng học", typeof(string)),
                        new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("Ngày bắt đầu",typeof(DateTime)), new DataColumn("Ngày kết thúc",typeof(DateTime)),new DataColumn("SLSV",typeof(int)),new DataColumn("Ghi chú",typeof(string))});
            string test = "";
            LoadDSLop();
            HienThiThongTinMon(ID, test);
        }
        void HienThiThongTinMon(string id, string test)
        {
            List<lecturecourse> courses = lecturecourseDAO.Instance.LoadSlecturecourseDAO(id);
            if (test == "")
            {
                foreach (lecturecourse course in courses)
                {
                    dt.Rows.Add(course.CourseId, course.CourseName, course.ClassRoom, course.Day, course.Period, course.StartDate, course.EndDate, course.SLSV, course.Ghichu);
                }
            }
            else
            {
                foreach (lecturecourse course in courses)
                {
                    if (course.CourseId.ToString() == test)
                        dt.Rows.Add(course.CourseId, course.CourseName, course.ClassRoom, course.Day, course.Period, course.StartDate, course.EndDate, course.SLSV, course.Ghichu);
                }
            }
            this.dgvcourse.DataSource = dt;
            for (int k = 0; k < dgvcourse.Columns.Count; k++)
            {
                dgvcourse.Columns[k].ReadOnly = true;
            }
            //this.dgvcourse.AllowUserToAddRows = false;

        }
        private void cboloccourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            if (cboloccourse.SelectedIndex == -1) return;
            if (cboloccourse.SelectedItem.ToString() == "Tất cả") HienThiThongTinMon(ID, "");
            string line = cboloccourse.SelectedItem.ToString();
            string[] item = line.Split('/');
            string mamon = item[0];
            HienThiThongTinMon(ID, mamon);
        }

        #endregion
        void LoadDSLop()
        {
            cbodiem.Items.Clear();
            cboloccourse.Items.Clear();
            cbothongke.Items.Clear();
            cboloccourse.Items.Add("Tất cả");
            txtmssv.Enabled = false;
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            List<lecturecourse> courses = lecturecourseDAO.Instance.LoadSlecturecourseDAO(ID);
            foreach (lecturecourse course in courses)
            {
                string line = course.CourseId.ToString() + "/" + course.CourseName.ToString();
                cbodiem.Items.Add(line);
                cboloccourse.Items.Add(line);
                cbothongke.Items.Add(line);
            }
        }

        #region load_tab_diem
        private void cbodiem_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtmssv.Enabled = false;
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            if (cbodiem.SelectedIndex == -1) return;

            string[] mon = cbodiem.SelectedItem.ToString().Split('/');
            string mamon = mon[0];
            HienThiThongTinDiem(mamon);
        }

        void HienThiThongTinDiem(string mamon)
        {
            lvscoreGV.Items.Clear();
            List<lecturescore> scores = lecturescoreDAO.Instance.LoadLectureScore(mamon);
            foreach (lecturescore score in scores)
            {
                ListViewItem lvi = new ListViewItem(score.StudentId);
                lvi.SubItems.Add(score.StudentName);
                lvi.SubItems.Add(score.ProcessScore.ToString());
                lvi.SubItems.Add(score.MidtermScore.ToString());
                lvi.SubItems.Add(score.PracticeScore.ToString());
                lvi.SubItems.Add(score.FinalScore.ToString());
                lvi.SubItems.Add(score.CourseScore.ToString());
                lvscoreGV.Items.Add(lvi);

            }
        }
        private void lvscoreGV_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnnhap.Enabled = true;
            if (lvscoreGV.SelectedItems.Count == 0) return;
            ListViewItem lvi = lvscoreGV.SelectedItems[0];
            txtmssv.Text = lvi.SubItems[0].Text;
            txtQT.Text = lvi.SubItems[2].Text;
            txtGK.Text = lvi.SubItems[3].Text;
            txtTH.Text = lvi.SubItems[4].Text;
            txtCK.Text = lvi.SubItems[5].Text;
        }

        private void btnnhap_Click(object sender, EventArgs e)
        {
            btnok.Enabled = true;
            if (txtQT.Text != null || txtGK.Text != null || txtTH.Text != null || txtCK.Text != null)
            {
                txtQT.Enabled = true;
                txtGK.Enabled = true;
                txtTH.Enabled = true;
                txtCK.Enabled = true;
            }
            else
            {
                if (cbodiem.SelectedItem == null) return;
                if (lvscoreGV.SelectedItems.Count == 0) return;

                txtQT.Enabled = true;
                txtGK.Enabled = true;
                txtTH.Enabled = true;
                txtCK.Enabled = true;
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            string[] mon = cbodiem.SelectedItem.ToString().Split('/');
            string mamon = mon[0];
            int kq = lecturescoreDAO.Instance.UpdateScore(mamon, txtmssv.Text.ToString(),
                float.Parse(txtQT.Text), float.Parse(txtGK.Text), float.Parse(txtCK.Text), float.Parse(txtTH.Text));

            HienThiThongTinDiem(mamon);
            btnok.Enabled = false;
        }

        #endregion

        #region load tab thong ke
        void Loadthongke()
        {
            cboloaidiem.Items.Clear();
            cboloaidiem.Items.Add("Điểm QT");
            cboloaidiem.Items.Add("Điểm TH");
            cboloaidiem.Items.Add("Điểm GK");
            cboloaidiem.Items.Add("Điểm CK");
            cboloaidiem.Items.Add("Điểm HP");

        }
        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("MSSV", typeof(string)), new DataColumn("Điểm", typeof(string)) });
            if (cbothongke.Items.Count == 0) return;
            if (cboloaidiem.Items.Count == 0) return;
            string[] mon = cbothongke.SelectedItem.ToString().Split('/');
            string mamon = mon[0];
            string diem = cboloaidiem.SelectedItem.ToString();
            chartthongke.Series["Điểm"].Points.Clear();
            List<lecturescore> thongke = lecturescoreDAO.Instance.LoadLectureScore(mamon);
            foreach (lecturescore t in thongke)
            {
                switch (diem)
                {
                    case "Điểm QT":
                        dt.Rows.Add(t.StudentId, t.ProcessScore);
                        break;
                    case "Điểm TH":
                        dt.Rows.Add(t.StudentId, t.PracticeScore); break;
                    case "Điểm GK":
                        dt.Rows.Add(t.StudentId, t.MidtermScore); break;
                    case "Điểm CK":
                        dt.Rows.Add(t.StudentId, t.FinalScore); break;
                    case "Điểm HP":
                        dt.Rows.Add(t.StudentId, t.CourseScore); break;
                    default:
                        break;
                }
            }
            chartthongke.ChartAreas["ChartArea1"].AxisY.Maximum = 10;
            chartthongke.ChartAreas["ChartArea1"].AxisY.Title = diem;
            chartthongke.ChartAreas["ChartArea1"].AxisX.Title = "Bảng thống kê " + diem + " của lớp " + mamon;
            chartthongke.ChartAreas["ChartArea1"].AxisY.Interval = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chartthongke.Series["Điểm"].Points.AddXY(dt.Rows[i]["MSSV"], dt.Rows[i]["Điểm"]);
            }
        }
        #endregion


        //private void tb_Filter_TextChanged(object sender, EventArgs e)
        //{
        //    dt.DefaultView.RowFilter = string.Format("[Tên môn học] like '%{0}%' or [Mã môn học] like '%{0}%'", tb_Filter.Text);
        //}

        private void LecturerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseWindow logOut = new CloseWindow();
            logOut.ShowDialog();
            e.Cancel = logOut.IsNotClosed;
        }


    }

}

