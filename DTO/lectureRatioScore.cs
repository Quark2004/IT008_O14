using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSV.DTO
{
    public class lectureRatioScore
    {
        private string courseId;
        private float ratioprocessScore;
        private float ratiomidtermScore;
        private float ratiopracticeScore;
        private float ratiofinalScore;

        public string CourseId { get => courseId; set => courseId = value; }
        public float RatioProcessScore { get => ratioprocessScore; set => ratioprocessScore = value; }
        public float RatioMidtermScore { get => ratiomidtermScore; set => ratiomidtermScore = value; }
        public float RatioPracticeScore { get => ratiopracticeScore; set => ratiopracticeScore = value; }
        public float RatioFinalScore { get => ratiofinalScore; set => ratiofinalScore = value; }

        public lectureRatioScore(DataRow row)
        {
            try
            {
                this.CourseId = row["Mã môn học"].ToString();
                this.RatioProcessScore = float.Parse(row["Tỉ lệ điểm quá trình"].ToString());
                this.RatioMidtermScore = float.Parse(row["Tỉ lệ điểm giữa kì"].ToString());
                this.RatioPracticeScore = float.Parse(row["Tỉ lệ điểm thực hành"].ToString());
                this.RatioFinalScore = float.Parse(row["Tỉ lệ điểm cuối kì"].ToString());
            }
            catch { }
        }
    }
}
