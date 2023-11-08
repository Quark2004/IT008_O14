using QLSV.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
	public class StudentInfo
	{
		private string id;
		private string name;
		private DateTime birthday;
		private string gender;
		private string trainingSystem;
		private string educationLevel;

		public string Id { get => id; set => id = value; }
		public string Name { get => name; set => name = value; }
		public string Gender { get => gender; set => gender = value; }
		public DateTime Birthday { get => birthday; set => birthday = value; }
		public string TrainingSystem { get => trainingSystem; set => trainingSystem = value; }
		public string EducationLevel { get => educationLevel; set => educationLevel = value; }

		public StudentInfo(DataRow row)
		{
			this.Id = row["MSSV"].ToString();
			this.Name = row["Tên"].ToString();
			this.Birthday = (DateTime)row["Ngày sinh"];
			this.Gender = row["Giới tính"].ToString();
			this.educationLevel = row["Bậc đào tạo"].ToString();
			this.trainingSystem = row["Hệ đào tạo"].ToString();
		}

		public StudentInfo(string id, string name, DateTime birthday, string gender, string educationLevel, string trainingSystem)
		{
			this.Id = id;
			this.Name = name;
			this.Birthday = birthday;
			this.Gender = gender;
			this.EducationLevel = educationLevel;
			this.TrainingSystem = trainingSystem;
		}
	}
}
