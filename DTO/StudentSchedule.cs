using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
	public class StudentSchedule
	{
		private string subID;
		private string subName;
		private string room;
		private DateTime startDate;
		private DateTime endDate;
		private string day;
		private string period;

		public string SubID { get => subID; set => subID = value; }
		public string SubName { get => subName; set => subName = value; }
		public string Room { get => room; set => room = value; }
		public DateTime StartDate { get => startDate; set => startDate = value; }
		public DateTime EndDate { get => endDate; set => endDate = value; }
		public string Period { get => period; set => period = value; }
		public string Day { get => day; set => day = value; }

		public StudentSchedule(DataRow row)
		{
			this.SubID = row["Mã môn học"].ToString();
			this.SubName = row["Tên môn học"].ToString();
			this.Room = row["Phòng học"].ToString();
			if (!Convert.IsDBNull(row["Ngày bắt đầu"]))
			{
				this.StartDate = Convert.ToDateTime(row["Ngày bắt đầu"]);
			}
			if (!Convert.IsDBNull(row["Ngày kết thúc"]))
			{
				this.EndDate = Convert.ToDateTime(row["Ngày kết thúc"]);
			}
			this.Period = row["Tiết"].ToString();
			this.Day = row["Thứ"].ToString();
		}

	}
}
