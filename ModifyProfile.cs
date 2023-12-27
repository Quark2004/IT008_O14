using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace QLSV
{
    public partial class ModifyProfile : Form
    {
        string ID;

        public ModifyProfile(string id)
        {
            InitializeComponent();
            ID = id;
            LoadProfileInfo();

        }

        private void LoadProfileInfo()
        {
            StudentInfo info = StudentInfoDAO.Instance.LoadStudentInfo(ID);
            tb_id.Text = info.Id;
            tb_name.Text = info.Name;
            dtp_birthDay.Text = info.Birthday != new DateTime() ? info.Birthday.ToString("MM/dd/yyyy") : "";
            cb_gender.SelectedItem = (object)info.Gender;
            cb_educationLevel.SelectedItem = (object)info.EducationLevel;
            cb_trainingSystem.SelectedItem = (object)info.TrainingSystem;
            Image avt = ConvertBytesToImage(info.Avatar);
            pbx_avt.Image = avt;
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

        public byte[] ConvertImageToBytes(Image img)
        {
            if (img == null) { return null; }

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            byte[] avt = ConvertImageToBytes(pbx_avt.Image);

            string query = "SELECT UpdateProfile( :id , :name , :birthday , :gender , :educationLevel ,  :trainingSystem , :avt );";
            bool success = (bool)DataProvider.Instance.ExcuteScalar(query, new object[] { tb_id.Text, tb_name.Text, dtp_birthDay.Value.Date, cb_gender.SelectedItem.ToString(), cb_educationLevel.SelectedItem.ToString(), cb_trainingSystem.SelectedItem.ToString(), avt });

            if (success)
            {
                MessageBox.Show("Cập nhật thông tin cá nhân thành công!", "Cập nhật thông tin cá nhân", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Thông tin ngày sinh không hợp lệ", "Cập nhật thông tin cá nhân", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
