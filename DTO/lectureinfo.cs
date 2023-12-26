using System;
using System.Data;

namespace QLSV.DTO
{
    public class lectureinfo
    {
        private string id;
        private string name;
        private DateTime birthday;
        private string gender;
        private string trainingSystem;
        private string educationLevel;
        private byte[] avatar;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Gender { get => gender; set => gender = value; }
        public DateTime Birthday { get => birthday; set => birthday = value; }
        public string TrainingSystem { get => trainingSystem; set => trainingSystem = value; }
        public string EducationLevel { get => educationLevel; set => educationLevel = value; }
        public byte[] Avatar { get => avatar; set => avatar = value; }

        public lectureinfo(DataRow row)
        {
            try
            {
                this.Id = row["MSSV"].ToString();
                this.Name = row["Tên"].ToString();
                if (!Convert.IsDBNull(row["Ngày sinh"]))
                {
                    this.Birthday = Convert.ToDateTime(row["Ngày sinh"]);
                }
                else
                {
                    this.Birthday = new DateTime();
                }
                this.Gender = row["Giới tính"].ToString();
                this.EducationLevel = row["Bậc đào tạo"].ToString();
                this.TrainingSystem = row["Hệ đào tạo"].ToString();

                if (!Convert.IsDBNull(row["Ảnh đại diện"]))
                {
                    this.Avatar = (byte[])row["Ảnh đại diện"];
                }

            }
            catch
            {

            }
        }

        public lectureinfo(string id, string name, DateTime birthday, string gender, string educationLevel, string trainingSystem, byte[] avatar)
        {
            this.Id = id;
            this.Name = name;
            this.Birthday = birthday;
            this.Gender = gender;
            this.EducationLevel = educationLevel;
            this.TrainingSystem = trainingSystem;
            this.Avatar = avatar;
        }
    }
}
