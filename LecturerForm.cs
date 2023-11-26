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
            disable();
            enable();
            //LoadTKB();
        }
        void enable()
        {
            txtbirth.Enabled = true;
            txtname.Enabled = true;
            txtmakhoa.Enabled = true;
            txtkhoa.Enabled = true;
            txtid.Enabled = true;
            cbogender.Enabled = true;
        }
        void disable()
        {
            txtbirth.Enabled= false;
            txtname.Enabled= false;
            txtmakhoa.Enabled= false;
            txtkhoa.Enabled= false;
            txtid.Enabled= false;
            cbogender.Enabled= false;
        }
        public string ID { get; set; }

        #region load_tab_lichday
        void Loadlichday()
        {
            //List<lecturecourse> courses = lecturecourseDAO.Instance.LoadSlecturecourseDAO(ID);
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Mã môn học", typeof(string)),
                        new DataColumn("Tên môn học", typeof(string)),
                        new DataColumn("Thứ",typeof(string)),new DataColumn("Tiết",typeof(string)),new DataColumn("SLSV",typeof(string)),new DataColumn("Ghi chú",typeof(string))});
            string test = "";
            LoadDSLop();
            HienThiThongTinMon(ID, test);
        }
        void HienThiThongTinMon (string id, string test)
        {
            List<lecturecourse> courses = lecturecourseDAO.Instance.LoadSlecturecourseDAO(id);
            if (test == "")
            {
                foreach (lecturecourse course in courses)
                {
                    dt.Rows.Add(course.CourseId, course.CourseName, course.Day, course.Period, course.Slsv, course.Note);
                }
            }
            else
            {
                foreach (lecturecourse course in courses)
                {
                    if (course.CourseId.ToString() == test)
                        dt.Rows.Add(course.CourseId, course.CourseName, course.Day, course.Period, course.Slsv, course.Note);
                }
            }
            this.dgvcourse.DataSource = dt;

        }
        private void cboloccourse_SelectedIndexChanged(object sender, EventArgs e)
        {

            dt.Rows.Clear();
            if (cboloccourse.SelectedIndex == -1) return;
            if (cboloccourse.SelectedItem.ToString() == "Tất cả") HienThiThongTinMon(ID,"");
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
            txtQT.Enabled = true;
            txtGK.Enabled = true;
            txtTH.Enabled = true;
            txtCK.Enabled = true;
            
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            int kq = lecturescoreDAO.Instance.UpdateScore(cbodiem.SelectedItem.ToString(), txtmssv.Text, txtQT.Text, txtGK.Text, txtTH.Text, txtCK.Text);
            if (kq > 0)
            {
                cbodiem_SelectedIndexChanged(sender, e);
            }
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            
        }
        #endregion
    }

}

