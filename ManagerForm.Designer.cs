namespace QLSV {
	partial class ManagerForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerForm));
			this.tabCtrl_main = new System.Windows.Forms.TabControl();
			this.tab_account = new System.Windows.Forms.TabPage();
			this.tab_course = new System.Windows.Forms.TabPage();
			this.tab_registration = new System.Windows.Forms.TabPage();
			this.btn_openRegistration = new System.Windows.Forms.Button();
			this.btn_modify = new System.Windows.Forms.Button();
			this.btn_export = new System.Windows.Forms.Button();
			this.btn_import = new System.Windows.Forms.Button();
			this.panel_ = new System.Windows.Forms.Panel();
			this.data_allCourse = new System.Windows.Forms.DataGridView();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel_option = new System.Windows.Forms.Panel();
			this.lb_find = new System.Windows.Forms.Label();
			this.tb_findStudent = new System.Windows.Forms.TextBox();
			this.btn_accept = new System.Windows.Forms.Button();
			this.panel_courseList = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.tb_studentId = new System.Windows.Forms.TextBox();
			this.tb_studentName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.data_courseListOfStudent = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.panel_studentList = new System.Windows.Forms.Panel();
			this.data_studentList = new System.Windows.Forms.DataGridView();
			this.tabCtrl_main.SuspendLayout();
			this.tab_registration.SuspendLayout();
			this.panel_.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.data_allCourse)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.panel_option.SuspendLayout();
			this.panel_courseList.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.data_courseListOfStudent)).BeginInit();
			this.panel_studentList.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.data_studentList)).BeginInit();
			this.SuspendLayout();
			// 
			// tabCtrl_main
			// 
			this.tabCtrl_main.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabCtrl_main.Controls.Add(this.tab_account);
			this.tabCtrl_main.Controls.Add(this.tab_course);
			this.tabCtrl_main.Controls.Add(this.tab_registration);
			this.tabCtrl_main.Controls.Add(this.tabPage1);
			this.tabCtrl_main.Font = new System.Drawing.Font("Cambria", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabCtrl_main.Location = new System.Drawing.Point(1, 0);
			this.tabCtrl_main.Name = "tabCtrl_main";
			this.tabCtrl_main.SelectedIndex = 0;
			this.tabCtrl_main.Size = new System.Drawing.Size(1367, 659);
			this.tabCtrl_main.TabIndex = 0;
			// 
			// tab_account
			// 
			this.tab_account.Font = new System.Drawing.Font("Cambria", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tab_account.Location = new System.Drawing.Point(4, 38);
			this.tab_account.Name = "tab_account";
			this.tab_account.Padding = new System.Windows.Forms.Padding(3);
			this.tab_account.Size = new System.Drawing.Size(1359, 617);
			this.tab_account.TabIndex = 0;
			this.tab_account.Text = "Tài khoản";
			this.tab_account.UseVisualStyleBackColor = true;
			// 
			// tab_course
			// 
			this.tab_course.Location = new System.Drawing.Point(4, 38);
			this.tab_course.Name = "tab_course";
			this.tab_course.Padding = new System.Windows.Forms.Padding(3);
			this.tab_course.Size = new System.Drawing.Size(1359, 617);
			this.tab_course.TabIndex = 1;
			this.tab_course.Text = "Môn học";
			this.tab_course.UseVisualStyleBackColor = true;
			// 
			// tab_registration
			// 
			this.tab_registration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tab_registration.Controls.Add(this.btn_openRegistration);
			this.tab_registration.Controls.Add(this.btn_modify);
			this.tab_registration.Controls.Add(this.btn_export);
			this.tab_registration.Controls.Add(this.btn_import);
			this.tab_registration.Controls.Add(this.panel_);
			this.tab_registration.Location = new System.Drawing.Point(4, 38);
			this.tab_registration.Name = "tab_registration";
			this.tab_registration.Padding = new System.Windows.Forms.Padding(3);
			this.tab_registration.Size = new System.Drawing.Size(1359, 617);
			this.tab_registration.TabIndex = 2;
			this.tab_registration.Text = "   ĐKHP";
			this.tab_registration.UseVisualStyleBackColor = true;
			// 
			// btn_openRegistration
			// 
			this.btn_openRegistration.Location = new System.Drawing.Point(797, 534);
			this.btn_openRegistration.Name = "btn_openRegistration";
			this.btn_openRegistration.Size = new System.Drawing.Size(113, 67);
			this.btn_openRegistration.TabIndex = 3;
			this.btn_openRegistration.Text = "Mở ĐKHP";
			this.btn_openRegistration.UseVisualStyleBackColor = true;
			this.btn_openRegistration.Click += new System.EventHandler(this.btn_openRegistration_Click);
			// 
			// btn_modify
			// 
			this.btn_modify.Location = new System.Drawing.Point(1007, 534);
			this.btn_modify.Name = "btn_modify";
			this.btn_modify.Size = new System.Drawing.Size(113, 67);
			this.btn_modify.TabIndex = 4;
			this.btn_modify.Text = "Sửa";
			this.btn_modify.UseVisualStyleBackColor = true;
			this.btn_modify.Click += new System.EventHandler(this.btn_modify_Click);
			// 
			// btn_export
			// 
			this.btn_export.Location = new System.Drawing.Point(512, 534);
			this.btn_export.Name = "btn_export";
			this.btn_export.Size = new System.Drawing.Size(156, 67);
			this.btn_export.TabIndex = 2;
			this.btn_export.Text = "Xuất file ĐKHP";
			this.btn_export.UseVisualStyleBackColor = true;
			this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
			// 
			// btn_import
			// 
			this.btn_import.Location = new System.Drawing.Point(196, 534);
			this.btn_import.Name = "btn_import";
			this.btn_import.Size = new System.Drawing.Size(225, 67);
			this.btn_import.TabIndex = 1;
			this.btn_import.Text = "Nhập danh sách môn ĐKHP";
			this.btn_import.UseVisualStyleBackColor = true;
			this.btn_import.Click += new System.EventHandler(this.btn_import_Click);
			// 
			// panel_
			// 
			this.panel_.Controls.Add(this.data_allCourse);
			this.panel_.Location = new System.Drawing.Point(7, 6);
			this.panel_.Name = "panel_";
			this.panel_.Size = new System.Drawing.Size(1345, 510);
			this.panel_.TabIndex = 0;
			// 
			// data_allCourse
			// 
			this.data_allCourse.AllowUserToAddRows = false;
			this.data_allCourse.AllowUserToDeleteRows = false;
			this.data_allCourse.AllowUserToResizeColumns = false;
			this.data_allCourse.AllowUserToResizeRows = false;
			this.data_allCourse.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.data_allCourse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.data_allCourse.Location = new System.Drawing.Point(3, 3);
			this.data_allCourse.MultiSelect = false;
			this.data_allCourse.Name = "data_allCourse";
			this.data_allCourse.ReadOnly = true;
			this.data_allCourse.RowHeadersVisible = false;
			this.data_allCourse.RowHeadersWidth = 51;
			this.data_allCourse.RowTemplate.Height = 24;
			this.data_allCourse.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.data_allCourse.Size = new System.Drawing.Size(1339, 504);
			this.data_allCourse.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel_option);
			this.tabPage1.Controls.Add(this.panel_courseList);
			this.tabPage1.Controls.Add(this.panel_studentList);
			this.tabPage1.Location = new System.Drawing.Point(4, 38);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1359, 617);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Danh sách ĐKHP";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel_option
			// 
			this.panel_option.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel_option.Controls.Add(this.lb_find);
			this.panel_option.Controls.Add(this.tb_findStudent);
			this.panel_option.Controls.Add(this.btn_accept);
			this.panel_option.Location = new System.Drawing.Point(367, 3);
			this.panel_option.Name = "panel_option";
			this.panel_option.Size = new System.Drawing.Size(985, 112);
			this.panel_option.TabIndex = 4;
			// 
			// lb_find
			// 
			this.lb_find.AutoEllipsis = true;
			this.lb_find.AutoSize = true;
			this.lb_find.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lb_find.Location = new System.Drawing.Point(28, 42);
			this.lb_find.Name = "lb_find";
			this.lb_find.Size = new System.Drawing.Size(81, 27);
			this.lb_find.TabIndex = 4;
			this.lb_find.Text = "Lọc SV:";
			// 
			// tb_findStudent
			// 
			this.tb_findStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_findStudent.Location = new System.Drawing.Point(136, 41);
			this.tb_findStudent.Name = "tb_findStudent";
			this.tb_findStudent.Size = new System.Drawing.Size(326, 33);
			this.tb_findStudent.TabIndex = 3;
			this.tb_findStudent.TextChanged += new System.EventHandler(this.tb_findStudent_TextChanged);
			// 
			// btn_accept
			// 
			this.btn_accept.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_accept.Location = new System.Drawing.Point(620, 25);
			this.btn_accept.Name = "btn_accept";
			this.btn_accept.Size = new System.Drawing.Size(250, 61);
			this.btn_accept.TabIndex = 2;
			this.btn_accept.Text = "Chấp nhận học phần";
			this.btn_accept.UseVisualStyleBackColor = true;
			this.btn_accept.Click += new System.EventHandler(this.btn_accept_Click);
			// 
			// panel_courseList
			// 
			this.panel_courseList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel_courseList.Controls.Add(this.label3);
			this.panel_courseList.Controls.Add(this.tb_studentId);
			this.panel_courseList.Controls.Add(this.tb_studentName);
			this.panel_courseList.Controls.Add(this.label2);
			this.panel_courseList.Controls.Add(this.label1);
			this.panel_courseList.Controls.Add(this.data_courseListOfStudent);
			this.panel_courseList.Location = new System.Drawing.Point(367, 120);
			this.panel_courseList.Name = "panel_courseList";
			this.panel_courseList.Size = new System.Drawing.Size(985, 493);
			this.panel_courseList.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(377, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(199, 26);
			this.label3.TabIndex = 6;
			this.label3.Text = "Các môn đã đăng ký";
			// 
			// tb_studentId
			// 
			this.tb_studentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_studentId.Enabled = false;
			this.tb_studentId.Location = new System.Drawing.Point(620, 32);
			this.tb_studentId.Name = "tb_studentId";
			this.tb_studentId.Size = new System.Drawing.Size(151, 33);
			this.tb_studentId.TabIndex = 5;
			// 
			// tb_studentName
			// 
			this.tb_studentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_studentName.Enabled = false;
			this.tb_studentName.Location = new System.Drawing.Point(136, 32);
			this.tb_studentName.Name = "tb_studentName";
			this.tb_studentName.Size = new System.Drawing.Size(326, 33);
			this.tb_studentName.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(525, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 26);
			this.label2.TabIndex = 3;
			this.label2.Text = "MSSV:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(29, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "Họ tên:";
			// 
			// data_courseListOfStudent
			// 
			this.data_courseListOfStudent.AllowUserToAddRows = false;
			this.data_courseListOfStudent.AllowUserToDeleteRows = false;
			this.data_courseListOfStudent.AllowUserToResizeColumns = false;
			this.data_courseListOfStudent.AllowUserToResizeRows = false;
			this.data_courseListOfStudent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.data_courseListOfStudent.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.data_courseListOfStudent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.data_courseListOfStudent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
			this.data_courseListOfStudent.Location = new System.Drawing.Point(3, 141);
			this.data_courseListOfStudent.Name = "data_courseListOfStudent";
			this.data_courseListOfStudent.RowHeadersVisible = false;
			this.data_courseListOfStudent.RowHeadersWidth = 51;
			this.data_courseListOfStudent.RowTemplate.Height = 24;
			this.data_courseListOfStudent.Size = new System.Drawing.Size(977, 348);
			this.data_courseListOfStudent.TabIndex = 1;
			this.data_courseListOfStudent.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.data_courseListOfStudent_DataBindingComplete);
			// 
			// Column1
			// 
			this.Column1.HeaderText = "";
			this.Column1.MinimumWidth = 6;
			this.Column1.Name = "Column1";
			this.Column1.Width = 6;
			// 
			// panel_studentList
			// 
			this.panel_studentList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel_studentList.Controls.Add(this.data_studentList);
			this.panel_studentList.Location = new System.Drawing.Point(3, 3);
			this.panel_studentList.Name = "panel_studentList";
			this.panel_studentList.Size = new System.Drawing.Size(358, 611);
			this.panel_studentList.TabIndex = 0;
			// 
			// data_studentList
			// 
			this.data_studentList.AllowUserToAddRows = false;
			this.data_studentList.AllowUserToDeleteRows = false;
			this.data_studentList.AllowUserToResizeColumns = false;
			this.data_studentList.AllowUserToResizeRows = false;
			this.data_studentList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.data_studentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.data_studentList.Location = new System.Drawing.Point(2, 2);
			this.data_studentList.Name = "data_studentList";
			this.data_studentList.ReadOnly = true;
			this.data_studentList.RowHeadersVisible = false;
			this.data_studentList.RowHeadersWidth = 51;
			this.data_studentList.RowTemplate.Height = 24;
			this.data_studentList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.data_studentList.Size = new System.Drawing.Size(351, 604);
			this.data_studentList.TabIndex = 0;
			this.data_studentList.SelectionChanged += new System.EventHandler(this.data_studentList_SelectionChanged);
			// 
			// ManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1369, 657);
			this.Controls.Add(this.tabCtrl_main);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Quản lý";
			this.tabCtrl_main.ResumeLayout(false);
			this.tab_registration.ResumeLayout(false);
			this.panel_.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.data_allCourse)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.panel_option.ResumeLayout(false);
			this.panel_option.PerformLayout();
			this.panel_courseList.ResumeLayout(false);
			this.panel_courseList.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.data_courseListOfStudent)).EndInit();
			this.panel_studentList.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.data_studentList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrl_main;
		private System.Windows.Forms.TabPage tab_account;
		private System.Windows.Forms.TabPage tab_course;
		private System.Windows.Forms.TabPage tab_registration;
		private System.Windows.Forms.Panel panel_;
		private System.Windows.Forms.Button btn_export;
		private System.Windows.Forms.Button btn_import;
		private System.Windows.Forms.DataGridView data_allCourse;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel_studentList;
		private System.Windows.Forms.Panel panel_courseList;
		private System.Windows.Forms.DataGridView data_courseListOfStudent;
		private System.Windows.Forms.DataGridView data_studentList;
		private System.Windows.Forms.Button btn_accept;
		private System.Windows.Forms.Panel panel_option;
		private System.Windows.Forms.Label lb_find;
		private System.Windows.Forms.TextBox tb_findStudent;
		private System.Windows.Forms.Button btn_modify;
		private System.Windows.Forms.TextBox tb_studentId;
		private System.Windows.Forms.TextBox tb_studentName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btn_openRegistration;
	}
}