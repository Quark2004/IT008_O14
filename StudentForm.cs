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
	public partial class StudentForm : Form
	{

		public StudentForm(string id)
		{
			InitializeComponent();
			ID = id;
			LoadInfo();
		}

		public string ID { get; set; }

		#region Method
		void LoadInfo()
		{
			StudentInfo info = StudentInfoDAO.Instance.LoadStudentInfo(ID);
			lb_ID.Text = info.Id;
			lb_Name.Text = info.Name;
			lb_Birthday.Text = info.Birthday.ToShortDateString();
			lb_Class.Text = info.ClassId;
			lb_Gender.Text = info.Gender;
		}
		#endregion

	}
}
