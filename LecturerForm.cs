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
    public partial class LecturerForm : Form
    {
        public LecturerForm(string id)
        {
            InitializeComponent();
            ID = id;
            //LoadTKB();
        }

        public string ID { get; set; }
        //private void LoadTKB()
        //{
        //    int cellWidth = 125;
        //    int cellHeight = 45;

        //    string[] timeline = { "7:30 - 8:15", "8:15 - 9:00", "9:00 - 9:45", "10:00 - 10:45", "10:45 - 11:30", "13:00 - 13:45", "13:45 - 14:30", "14:30 - 15:15", "15:30 - 16:15", "16:15 - 17:00" };

        //    int courseIndex = 0;

        //    for (int col = 1; col <= 7; col++)
        //    {
        //        for (int row = 0; row <= 10; row++)
        //        {
        //            Label lb = new Label
        //            {
        //                Width = cellWidth,
        //                Height = cellHeight,

        //                BorderStyle = BorderStyle.FixedSingle,

        //                Margin = new Padding(0),
        //                TextAlign = ContentAlignment.MiddleCenter,

        //            };

        //            flptkb.Controls.Add(lb);


        //            if (row == 0 && col == 1)
        //            {
        //                lb.Text = "Thứ/Tiết";
        //            }
        //            else if (row == 0)
        //            {
        //                lb.Text = $"Thứ {col}";
        //            }
        //            else if (col == 1)
        //            {
        //                lb.Text = $"Tiết {row}\n({timeline[row - 1]})";

        //            }

        //            if (col == 1)
        //            {
        //                lb.Width = 130;
        //            }

        //        }
        //    }
        //}
    }

}

