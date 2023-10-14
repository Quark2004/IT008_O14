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
		private string birthday;
		private string gender;
		private string classId;

		public string Id { get => id; set => id = value; }
		public string Name { get => name; set => name = value; }
		public string Birthday { get => birthday; set => birthday = value; }
		public string Gender { get => gender; set => gender = value; }
		public string ClassId { get => classId; set => classId = value; }

		public StudentInfo(DataRow row)
		{
			this.Id = row["student_id"].ToString();
			this.Name = row["name"].ToString();
			this.Birthday = row["birthday"].ToString();
			this.Gender = row["gender"].ToString();
			this.ClassId = row["class_id"].ToString();
		}

		public StudentInfo(string id, string name, string birthday, string gender, string classId)
		{
			this.Id = id;
			this.Name = name;
			this.Birthday = birthday;
			this.Gender = gender;
			this.ClassId = classId;
		}
	}
}
