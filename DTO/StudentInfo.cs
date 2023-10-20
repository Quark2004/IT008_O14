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
		private string classId;
		private string department;

		public string Id { get => id; set => id = value; }
		public string Name { get => name; set => name = value; }
		public string Gender { get => gender; set => gender = value; }
		public string ClassId { get => classId; set => classId = value; }
		public DateTime Birthday { get => birthday; set => birthday = value; }
		public string Department { get => department; set => department = value; }

		public StudentInfo(DataRow row)
		{
			this.Id = row["student_id"].ToString();
			this.Name = row["name"].ToString();
			this.Birthday = (DateTime)row["birthday"];
			this.Gender = row["gender"].ToString();
			this.ClassId = row["class_id"].ToString();
			this.Department = row["department"].ToString();
		}

		public StudentInfo(string id, string name, DateTime birthday, string gender, string classId, string department)
		{
			this.Id = id;
			this.Name = name;
			this.Birthday = birthday;
			this.Gender = gender;
			this.ClassId = classId;
			this.Department = department;
		}
	}
}
