using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    public class lecturecourse
    {
        private string courseName;
        private string courseId;
        private string day;
        private string period;
        private string classRoom;
        private DateTime startDate;
        private DateTime endDate;
        private int slsv;
        private string ghichu;
        public string CourseName { get => courseName; set => courseName = value; }
        public string CourseId { get => courseId; set => courseId = value; }
        public string Ghichu { get => ghichu; set => ghichu = value; }
        public int SLSV { get => slsv; set => slsv = value; }
        public string Day { get => day; set => day = value; }
        public string Period { get => period; set => period = value; }
        public string ClassRoom { get => classRoom; set => classRoom = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }

        public lecturecourse(DataRow row)
        {
            this.CourseName = row["Tên môn học"].ToString();
            this.CourseId = row["Mã môn học"].ToString();
            this.Ghichu = row["Ghi chú"].ToString();
            this.SLSV = int.Parse(row["SLSV"].ToString());
            this.Day = row["Thứ"].ToString();
            this.Period = row["Tiết"].ToString();
            this.ClassRoom = row["Phòng học"].ToString();
            if (!Convert.IsDBNull(row["Ngày bắt đầu"]))
            {
                this.StartDate = Convert.ToDateTime(row["Ngày bắt đầu"]);
            }
            if (!Convert.IsDBNull(row["Ngày kết thúc"]))
            {
                this.EndDate = Convert.ToDateTime(row["Ngày kết thúc"]);
            }
        }
    }
}
