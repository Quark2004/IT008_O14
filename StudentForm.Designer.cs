namespace QLSV
{
	partial class StudentForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabCtr_Student = new System.Windows.Forms.TabControl();
			this.tab_Score = new System.Windows.Forms.TabPage();
			this.tab_Schedule = new System.Windows.Forms.TabPage();
			this.tab_courseRegister = new System.Windows.Forms.TabPage();
			this.tabCtr_Student.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtr_Student
			// 
			this.tabCtr_Student.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabCtr_Student.Controls.Add(this.tab_Score);
			this.tabCtr_Student.Controls.Add(this.tab_Schedule);
			this.tabCtr_Student.Controls.Add(this.tab_courseRegister);
			this.tabCtr_Student.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabCtr_Student.ItemSize = new System.Drawing.Size(120, 32);
			this.tabCtr_Student.Location = new System.Drawing.Point(12, 12);
			this.tabCtr_Student.Name = "tabCtr_Student";
			this.tabCtr_Student.SelectedIndex = 0;
			this.tabCtr_Student.ShowToolTips = true;
			this.tabCtr_Student.Size = new System.Drawing.Size(776, 426);
			this.tabCtr_Student.TabIndex = 0;
			// 
			// tab_Score
			// 
			this.tab_Score.BackColor = System.Drawing.Color.LightGray;
			this.tab_Score.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.tab_Score.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tab_Score.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tab_Score.ForeColor = System.Drawing.SystemColors.Control;
			this.tab_Score.Location = new System.Drawing.Point(4, 36);
			this.tab_Score.Name = "tab_Score";
			this.tab_Score.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Score.Size = new System.Drawing.Size(768, 386);
			this.tab_Score.TabIndex = 0;
			this.tab_Score.Text = "    Điểm";
			// 
			// tab_Schedule
			// 
			this.tab_Schedule.BackColor = System.Drawing.Color.LightGray;
			this.tab_Schedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tab_Schedule.Location = new System.Drawing.Point(4, 36);
			this.tab_Schedule.Name = "tab_Schedule";
			this.tab_Schedule.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Schedule.Size = new System.Drawing.Size(768, 386);
			this.tab_Schedule.TabIndex = 1;
			this.tab_Schedule.Text = "Lịch học";
			// 
			// tab_courseRegister
			// 
			this.tab_courseRegister.BackColor = System.Drawing.Color.LightGray;
			this.tab_courseRegister.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tab_courseRegister.Location = new System.Drawing.Point(4, 36);
			this.tab_courseRegister.Name = "tab_courseRegister";
			this.tab_courseRegister.Padding = new System.Windows.Forms.Padding(3);
			this.tab_courseRegister.Size = new System.Drawing.Size(768, 386);
			this.tab_courseRegister.TabIndex = 2;
			this.tab_courseRegister.Text = "Đăng ký học phần";
			// 
			// StudentForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tabCtr_Student);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "StudentForm";
			this.Text = "Sinh Viên";
			this.Load += new System.EventHandler(this.StudentForm_Load);
			this.tabCtr_Student.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtr_Student;
		private System.Windows.Forms.TabPage tab_Score;
		private System.Windows.Forms.TabPage tab_Schedule;
		private System.Windows.Forms.TabPage tab_courseRegister;
	}
}