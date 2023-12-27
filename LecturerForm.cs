using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel;
using System.IO;
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
            Loadinfo();
            Loadthongke();
            Loaddiem();
            btnok.Enabled = false;
            btnnhap.Enabled = false;
            btn_huy.Enabled = false;

        }

        public string ID { get; set; }

        void Loadinfo()
        {
            lectureinfo info = lectureinfoDAO.Instance.LoadLectureInfo(ID);
            lbid.Text = info.Id;
            lbname.Text = info.Name;
            lbbirth.Text = info.Birthday != new DateTime() ? info.Birthday.ToString("MM/dd/yyyy") : "";
            lbgender.Text = info.Gender;
            lbkhoa.Text = info.EducationLevel;
            lbmakhoa.Text = info.TrainingSystem;
            Image avt = ConvertBytesToImage(info.Avatar);
            pb_avt.Image = avt;
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

        #endregion
        void LoadDSLop()
        {
            cbodiem.Items.Clear();
            cbothongke.Items.Clear();
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
                cbothongke.Items.Add(line);
            }
        }

        #region load_tab_diem
        private bool check(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;

            if (float.TryParse(s, out float diem))
            {
                if (diem >= 0 && diem <= 10)
                {
                    return true;
                }
            }
            return false;
        }

        ListViewItem ratio;
        void loadratio(string course)
        {
            
            List<lectureRatioScore> ratioscores = lectureRatioScoreDAO.Instance.LoadRatioCourse(course);
            foreach (lectureRatioScore ratioscore in ratioscores)
            {
                ratio = new ListViewItem(ratioscore.CourseId);
                ratio.SubItems.Clear();
                ratio.SubItems.Add(ratioscore.RatioProcessScore.ToString());
                ratio.SubItems.Add(ratioscore.RatioMidtermScore.ToString());
                ratio.SubItems.Add(ratioscore.RatioPracticeScore.ToString());
                ratio.SubItems.Add(ratioscore.RatioFinalScore.ToString());

            }
        }
        void Loaddiem()
        {
            cbodiem.Text = cbodiem.Items[0].ToString();
            string[] mon = cbodiem.Items[0].ToString().Split('/');
            string mamon = mon[0];
            HienThiThongTinDiem(mamon);
            loadratio(mamon);
        }
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
                //MessageBox.Show(score.ProcessScore.ToString());
                lvi.SubItems.Add(score.StudentName);


                if (score.ProcessScore == -1)
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(score.ProcessScore.ToString());
                if (score.MidtermScore == -1)
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(score.MidtermScore.ToString());
                if (score.PracticeScore == -1)
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(score.PracticeScore.ToString());
                if (score.FinalScore == -1)
                    lvi.SubItems.Add("");
                else
                    lvi.SubItems.Add(score.FinalScore.ToString());
                if (score.CourseScore == -1)
                    lvi.SubItems.Add("");
                else
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
            temp = lvi;
        }

        

        private void btnnhap_Click(object sender, EventArgs e)
        {
            btnok.Enabled = true;
            btn_huy.Enabled = true;
            btnnhap.Enabled = false;
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
            btnnhap.Enabled = false;
            btn_huy.Enabled = false;
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            string[] mon = cbodiem.SelectedItem.ToString().Split('/');
            string mamon = mon[0];
            if (ratiosco)
            {   
                
            }
            else
            {
                if (!check(txtQT.Text) || !check(txtGK.Text) || !check(txtCK.Text) || !check(txtTH.Text))
                {
                    diemKhongHopLe(lvscoreGV.SelectedItems[0]);
                }
                else
                {
                    //xoadiem();
                    string kq = lecturescoreDAO.Instance.UpdateScore(mamon, txtmssv.Text.ToString(),
                       txtQT.Text, txtGK.Text, txtCK.Text, txtTH.Text);
                    HienThiThongTinDiem(mamon);
                    btnok.Enabled = false;
                }
            }
        }

        private void diemKhongHopLe(ListViewItem item)
        {
            txtQT.Text = item.SubItems[2].Text;
            txtGK.Text = item.SubItems[3].Text;
            txtTH.Text = item.SubItems[4].Text;
            txtCK.Text = item.SubItems[5].Text;
            MessageBox.Show("Điểm nhập vào không hợp lệ!", "Cập nhật điểm", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnok.Enabled = false;
        }

        ListViewItem temp;
        private void btn_huy_Click(object sender, EventArgs e)
        {
            btn_huy.Enabled = false;
            btnnhap.Enabled = false;
            btnok.Enabled = false;
            txtQT.Enabled = false;
            txtGK.Enabled = false;
            txtTH.Enabled = false;
            txtCK.Enabled = false;
            temp = lvscoreGV.SelectedItems[0];
            txtQT.Text = temp.SubItems[2].Text;
            txtGK.Text = temp.SubItems[3].Text;
            txtTH.Text = temp.SubItems[4].Text;
            txtCK.Text = temp.SubItems[5].Text;
        }

        bool ratiosco = false;
        private void btnratio_Click(object sender, EventArgs e)
        {
            ratiosco = true;
            txtmssv.Text = "";
            txtQT.Text = ratio.SubItems[1].Text;
            txtGK.Text = ratio.SubItems[2].Text;
            txtTH.Text = ratio.SubItems[3].Text;
            txtCK.Text = ratio.SubItems[4].Text;



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

        private int check(float t)
        {
            if (t < 5.0)
                return 0;
            if (t >= 5.0 && t <= 6.4)
                return 1;
            if (t >= 6.5 && t <= 7.9)
                return 2;
            if (t >= 8.0)
                return 3;
            return 4;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Thang điểm", typeof(string)), new DataColumn("SL", typeof(int)) });
            if (cbothongke.Items.Count == 0) return;
            if (cboloaidiem.Items.Count == 0) return;
            string[] mon = cbothongke.SelectedItem.ToString().Split('/');
            string mamon = mon[0];
            string diem = cboloaidiem.SelectedItem.ToString();
            dt.Rows.Add("0 - 4.9", 0);
            dt.Rows.Add("5.0 - 6.4", 0);
            dt.Rows.Add("6.5 - 7.9", 0);
            dt.Rows.Add("8.0 - 10", 0);
            chartthongke.Series["Điểm"].Points.Clear();
            float temp;
            List<lecturescore> thongke = lecturescoreDAO.Instance.LoadLectureScore(mamon);
            foreach (lecturescore t in thongke)
            {
                switch (diem)
                {
                    case "Điểm QT":
                        temp = float.Parse(t.ProcessScore.ToString());

                        dt.Rows[check(temp)]["SL"] = (int)dt.Rows[check(temp)]["SL"] + 1;
                        break;
                    case "Điểm TH":
                        temp = float.Parse(t.PracticeScore.ToString());
                        dt.Rows[check(temp)]["SL"] = (int)dt.Rows[check(temp)]["SL"] + 1;
                        break;
                    case "Điểm GK":
                        temp = float.Parse(t.MidtermScore.ToString());
                        dt.Rows[check(temp)]["SL"] = (int)dt.Rows[check(temp)]["SL"] + 1;
                        break;
                    case "Điểm CK":
                        temp = float.Parse(t.FinalScore.ToString());
                        dt.Rows[check(temp)]["SL"] = (int)dt.Rows[check(temp)]["SL"] + 1;
                        break;
                    case "Điểm HP":
                        temp = float.Parse(t.CourseScore.ToString());
                        dt.Rows[check(temp)]["SL"] = (int)dt.Rows[check(temp)]["SL"] + 1;
                        break;
                    default:
                        break;
                }
            }
            chartthongke.ChartAreas["ChartArea1"].AxisX.Title = "Thống kê thang điểm lớp " + mamon;
            chartthongke.ChartAreas["ChartArea1"].AxisY.Title = "Số lượng";
            chartthongke.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chartthongke.Series["Điểm"].Points.AddXY(dt.Rows[i]["Thang điểm"], dt.Rows[i]["SL"]);
            }
        }
        #endregion

        #region doi avt
        public byte[] ConvertImageToBytes(Image img)
        {
            if (img == null) { return null; }

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public Image ConvertBytesToImage(byte[] data)
        {
            if (data != null)
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
            }
            Image img = Image.FromFile(@"../../Resources/null_avt.png");
            return img;
        }

        private void btn_avt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Cập nhật ảnh thành công!", "Cập nhật ảnh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pb_avt.Image = Image.FromFile(openFileDialog.FileName);
                byte[] avt = ConvertImageToBytes(pb_avt.Image);
                lectureinfo info = lectureinfoDAO.Instance.LoadLectureInfo(ID);
                string query = "SELECT UpdateProfile( :id , :name , :birthday , :gender , :trainingSystem , :educationLevel , :avt );";
                DataProvider.Instance.ExcuteNonQuery(query, new object[] { info.Id, info.Name, info.Birthday, info.Gender, info.TrainingSystem, info.EducationLevel, avt });
                Refresh();
            }
        }
        #endregion

        private void LecturerForm_FormClosing(object sender, FormClosingEventArgs e)
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


        private void txt_loc_TextChanged(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = string.Format("[Tên môn học] like '%{0}%' or [Mã môn học] like '%{0}%'", txt_loc.Text);
        }

        
    }

}

