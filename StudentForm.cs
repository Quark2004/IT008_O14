using QLSV.DAO;
using QLSV.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
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
            LoadSchedule();
            LoadTKB();
            data_Schedule.RowCount = 10;
            string[] schedule = { "(7:30 - 8:15)", "(8:15 - 9:00)", "(9:00 - 9:45)", "(10:00 - 10:45)", "(10:45 - 11:30)", "(13:00 - 13:45)", "(13:45 - 14:30)", "(14:30 - 15:15)", "(15:30 - 16:15)", "(16:15 - 17:00)" };
            for (int i = 1; i <= 10; i++)
            {
                DataGridViewRow row = data_Schedule.Rows[i - 1];
                row.Cells[0].Value = "Tiết " + i + "\n" + schedule[i - 1];
            }
        }

        public string ID { get; set; }

        #region Method
        void LoadInfo()
        {
            StudentInfo info = StudentInfoDAO.Instance.LoadStudentInfo(ID);
            lb_ID.Text = info.Id;
            lb_Name.Text = info.Name;
            lb_Birthday.Text = info.Birthday.ToShortDateString();
            lb_Gender.Text = info.Gender;
            lb_educationLevel.Text = info.EducationLevel;
            lb_TrainingSystem.Text = info.TrainingSystem;

            byte[] avtBytes = Encoding.ASCII.GetBytes(info.Avatar);
            string avt = "avt.jpg";
            File.WriteAllBytes(avt, avtBytes);
            pictureBox1.ImageLocation = avt;
        }

        void LoadSchedule()
        {
            data_Schedule.RowCount = 10;
            List<StudentSchedule> schedules = StudentScheduleDAO.Instance.LoadStudentSchedule(ID);

            foreach (StudentSchedule schedule in schedules)
            {
                int day = int.Parse(schedule.Day);

                List<string> result = new List<string>();
                for (int i = 10; i >= 1; i--)
                {
                    while (schedule.Period.Contains(i.ToString()))
                    {
                        result.Add(i.ToString());
                        schedule.Period = schedule.Period.Remove(schedule.Period.IndexOf(i.ToString()), i.ToString().Length);
                    }
                }

                List<int> periods = result.ConvertAll(int.Parse);

                foreach (int period in periods)
                {
                    DataGridViewRow row = data_Schedule.Rows[period - 1];
                    row.Cells[day - 1].Value = schedule.SubID + "\n" + schedule.SubName + "\n" + "P " + schedule.Room + "\n" + "BĐ: " + schedule.StartDate.ToShortDateString() + "\n" + "KT: " + schedule.EndDate.ToShortDateString() + "\n";
                }
            }
        }
        #endregion

        bool IsTheSameCellValue(int column, int row)
        {
            DataGridViewCell cell1 = data_Schedule[column, row];
            DataGridViewCell cell2 = data_Schedule[column, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }
            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void lv_Score_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = lv_Score.Columns[e.ColumnIndex].Width;
        }

        private void data_Schedule_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = data_Schedule.AdvancedCellBorderStyle.Top;
            }
        }

        private void data_Schedule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
                return;
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.Value = "";
                e.FormattingApplied = true;
            }

            foreach (DataGridViewRow row in data_Schedule.Rows)
            {
                for (int i = 0; i <= 6; i++)
                {
                    if (row.Cells[i].Value == null || i == 0)
                    {
                        row.Cells[i].Style.BackColor = Color.LightGray;
                    }
                }
            }
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            data_Schedule.AutoGenerateColumns = false;
        }

        private void LoadTKB()
        {
            List<StudentSchedule> schedules = StudentScheduleDAO.Instance.LoadStudentSchedule(ID);

            int cellWidth = 100;
            int cellHeight = 45;

            string[] timeline = { "7:30 - 8:15", "8:15 - 9:00", "9:00 - 9:45", "10:00 - 10:45", "10:45 - 11:30", "13:00 - 13:45", "13:45 - 14:30", "14:30 - 15:15", "15:30 - 16:15", "16:15 - 17:00" };

            int courseIndex = 0;

            for (int col = 1; col <= 7; col++)
            {
                for (int row = 0; row <= 10; row++)
                {
                    Label lb = new Label
                    {
                        Width = cellWidth,
                        Height = cellHeight,

                        BorderStyle = BorderStyle.FixedSingle,

                        Margin = new Padding(0),
                        TextAlign = ContentAlignment.MiddleCenter,

                    };

                    flpSchedule.Controls.Add(lb);


                    if (row == 0 && col == 1)
                    {
                        lb.Text = "Thứ/Tiết";
                    }
                    else if (row == 0)
                    {
                        lb.Text = $"Thứ {col}";
                    }
                    else if (col == 1)
                    {
                        lb.Text = $"Tiết {row}\n({timeline[row - 1]})";

                    }

                    if (col == 1)
                    {
                        lb.Width = 130;
                    }


                    if (courseIndex < schedules.Count)
                    {
                        StudentSchedule schedule = schedules[courseIndex];
                        int day = Convert.ToInt32(schedule.Day);
                        int startPeriod = schedule.Period[0] - '0';

                        if (day == col && startPeriod == row)
                        {
                            lb.Text = schedule.ToString();
                            lb.BackColor = Color.White;

                            lb.Height = cellHeight * schedule.Period.Length;
                            row = startPeriod + schedule.Period.Length - 1;
                            courseIndex++;
                        }
                    }
                }
            }
        }
    }
}
