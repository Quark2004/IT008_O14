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
            this.btn_acceptAccount = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_resetPass = new System.Windows.Forms.Button();
            this.btn_createAccount = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_idProfile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tb_passGenarator = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_role = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.lb_userName = new System.Windows.Forms.Label();
            this.data_accountsList = new System.Windows.Forms.DataGridView();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.data_profileList = new System.Windows.Forms.DataGridView();
            this.btn_editProfile = new System.Windows.Forms.Button();
            this.tabCtrl_main.SuspendLayout();
            this.tab_account.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_accountsList)).BeginInit();
            this.tab_registration.SuspendLayout();
            this.panel_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_allCourse)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.panel_option.SuspendLayout();
            this.panel_courseList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_courseListOfStudent)).BeginInit();
            this.panel_studentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_studentList)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_profileList)).BeginInit();
            this.SuspendLayout();
            // 
            // tabCtrl_main
            // 
            this.tabCtrl_main.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabCtrl_main.Controls.Add(this.tab_account);
            this.tabCtrl_main.Controls.Add(this.tab_course);
            this.tabCtrl_main.Controls.Add(this.tab_registration);
            this.tabCtrl_main.Controls.Add(this.tabPage1);
            this.tabCtrl_main.Controls.Add(this.tabPage2);
            this.tabCtrl_main.Font = new System.Drawing.Font("Cambria", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCtrl_main.Location = new System.Drawing.Point(1, 0);
            this.tabCtrl_main.Name = "tabCtrl_main";
            this.tabCtrl_main.SelectedIndex = 0;
            this.tabCtrl_main.Size = new System.Drawing.Size(1367, 659);
            this.tabCtrl_main.TabIndex = 0;
            // 
            // tab_account
            // 
            this.tab_account.Controls.Add(this.btn_acceptAccount);
            this.tab_account.Controls.Add(this.label10);
            this.tab_account.Controls.Add(this.btn_Cancel);
            this.tab_account.Controls.Add(this.btn_resetPass);
            this.tab_account.Controls.Add(this.btn_createAccount);
            this.tab_account.Controls.Add(this.panel4);
            this.tab_account.Controls.Add(this.panel3);
            this.tab_account.Controls.Add(this.panel1);
            this.tab_account.Controls.Add(this.panel2);
            this.tab_account.Controls.Add(this.data_accountsList);
            this.tab_account.Font = new System.Drawing.Font("Cambria", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_account.Location = new System.Drawing.Point(4, 38);
            this.tab_account.Name = "tab_account";
            this.tab_account.Padding = new System.Windows.Forms.Padding(3);
            this.tab_account.Size = new System.Drawing.Size(1359, 617);
            this.tab_account.TabIndex = 0;
            this.tab_account.Text = "Tài khoản";
            this.tab_account.UseVisualStyleBackColor = true;
            // 
            // btn_acceptAccount
            // 
            this.btn_acceptAccount.Enabled = false;
            this.btn_acceptAccount.Location = new System.Drawing.Point(919, 497);
            this.btn_acceptAccount.Name = "btn_acceptAccount";
            this.btn_acceptAccount.Size = new System.Drawing.Size(152, 72);
            this.btn_acceptAccount.TabIndex = 18;
            this.btn_acceptAccount.Text = "Xác nhận";
            this.btn_acceptAccount.UseVisualStyleBackColor = true;
            this.btn_acceptAccount.Click += new System.EventHandler(this.btn_acceptAccount_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Cambria", 30F);
            this.label10.Location = new System.Drawing.Point(704, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(443, 59);
            this.label10.TabIndex = 17;
            this.label10.Text = "Thông tin tài khoản";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Enabled = false;
            this.btn_Cancel.Location = new System.Drawing.Point(1086, 497);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(152, 72);
            this.btn_Cancel.TabIndex = 16;
            this.btn_Cancel.Text = "Hủy";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_resetPass
            // 
            this.btn_resetPass.Location = new System.Drawing.Point(742, 497);
            this.btn_resetPass.Name = "btn_resetPass";
            this.btn_resetPass.Size = new System.Drawing.Size(152, 72);
            this.btn_resetPass.TabIndex = 15;
            this.btn_resetPass.Text = "Đặt lại mật khẩu";
            this.btn_resetPass.UseVisualStyleBackColor = true;
            this.btn_resetPass.Click += new System.EventHandler(this.btn_resetPass_Click);
            // 
            // btn_createAccount
            // 
            this.btn_createAccount.Location = new System.Drawing.Point(565, 497);
            this.btn_createAccount.Name = "btn_createAccount";
            this.btn_createAccount.Size = new System.Drawing.Size(152, 72);
            this.btn_createAccount.TabIndex = 13;
            this.btn_createAccount.Text = "Tạo tài khoản";
            this.btn_createAccount.UseVisualStyleBackColor = true;
            this.btn_createAccount.Click += new System.EventHandler(this.btn_createAccount_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.tb_idProfile);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(565, 220);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(673, 69);
            this.panel4.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(326, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 27);
            this.label7.TabIndex = 2;
            this.label7.Text = "*";
            // 
            // tb_idProfile
            // 
            this.tb_idProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_idProfile.BackColor = System.Drawing.Color.White;
            this.tb_idProfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_idProfile.Enabled = false;
            this.tb_idProfile.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_idProfile.Location = new System.Drawing.Point(354, 18);
            this.tb_idProfile.MaxLength = 20;
            this.tb_idProfile.Name = "tb_idProfile";
            this.tb_idProfile.Size = new System.Drawing.Size(316, 34);
            this.tb_idProfile.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(211, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "MSSV/MGV";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tb_passGenarator);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Location = new System.Drawing.Point(565, 363);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(670, 62);
            this.panel3.TabIndex = 7;
            // 
            // tb_passGenarator
            // 
            this.tb_passGenarator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_passGenarator.BackColor = System.Drawing.Color.White;
            this.tb_passGenarator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_passGenarator.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_passGenarator.Location = new System.Drawing.Point(354, 17);
            this.tb_passGenarator.MaxLength = 20;
            this.tb_passGenarator.Name = "tb_passGenarator";
            this.tb_passGenarator.ReadOnly = true;
            this.tb_passGenarator.Size = new System.Drawing.Size(313, 34);
            this.tb_passGenarator.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(51, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(268, 23);
            this.label8.TabIndex = 0;
            this.label8.Text = "Mật khẩu được tạo ngẫu nhiên";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_role);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(565, 295);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 62);
            this.panel1.TabIndex = 6;
            // 
            // cb_role
            // 
            this.cb_role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_role.Enabled = false;
            this.cb_role.FormattingEnabled = true;
            this.cb_role.Items.AddRange(new object[] {
            "student",
            "teacher"});
            this.cb_role.Location = new System.Drawing.Point(354, 16);
            this.cb_role.Name = "cb_role";
            this.cb_role.Size = new System.Drawing.Size(181, 34);
            this.cb_role.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(326, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 27);
            this.label5.TabIndex = 2;
            this.label5.Text = "*";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(252, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "Vai trò";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.tb_username);
            this.panel2.Controls.Add(this.lb_userName);
            this.panel2.Location = new System.Drawing.Point(565, 145);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(673, 69);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(326, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 27);
            this.label4.TabIndex = 2;
            this.label4.Text = "*";
            // 
            // tb_username
            // 
            this.tb_username.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_username.BackColor = System.Drawing.Color.White;
            this.tb_username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_username.Enabled = false;
            this.tb_username.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_username.Location = new System.Drawing.Point(354, 18);
            this.tb_username.MaxLength = 20;
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(316, 34);
            this.tb_username.TabIndex = 1;
            // 
            // lb_userName
            // 
            this.lb_userName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_userName.AutoSize = true;
            this.lb_userName.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_userName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lb_userName.Location = new System.Drawing.Point(185, 29);
            this.lb_userName.Name = "lb_userName";
            this.lb_userName.Size = new System.Drawing.Size(134, 23);
            this.lb_userName.TabIndex = 0;
            this.lb_userName.Text = "Tên đăng nhập";
            // 
            // data_accountsList
            // 
            this.data_accountsList.AllowUserToAddRows = false;
            this.data_accountsList.AllowUserToDeleteRows = false;
            this.data_accountsList.AllowUserToResizeColumns = false;
            this.data_accountsList.AllowUserToResizeRows = false;
            this.data_accountsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.data_accountsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_accountsList.Location = new System.Drawing.Point(6, 7);
            this.data_accountsList.Name = "data_accountsList";
            this.data_accountsList.ReadOnly = true;
            this.data_accountsList.RowHeadersVisible = false;
            this.data_accountsList.RowHeadersWidth = 51;
            this.data_accountsList.RowTemplate.Height = 24;
            this.data_accountsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.data_accountsList.Size = new System.Drawing.Size(485, 604);
            this.data_accountsList.TabIndex = 1;
            this.data_accountsList.SelectionChanged += new System.EventHandler(this.data_accountsList_SelectionChanged);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_editProfile);
            this.tabPage2.Controls.Add(this.data_profileList);
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1359, 617);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Thông tin sinh viên";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // data_profileList
            // 
            this.data_profileList.AllowUserToAddRows = false;
            this.data_profileList.AllowUserToDeleteRows = false;
            this.data_profileList.AllowUserToResizeRows = false;
            this.data_profileList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.data_profileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_profileList.Location = new System.Drawing.Point(3, 6);
            this.data_profileList.MultiSelect = false;
            this.data_profileList.Name = "data_profileList";
            this.data_profileList.ReadOnly = true;
            this.data_profileList.RowHeadersVisible = false;
            this.data_profileList.RowHeadersWidth = 51;
            this.data_profileList.RowTemplate.Height = 24;
            this.data_profileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.data_profileList.Size = new System.Drawing.Size(1353, 542);
            this.data_profileList.TabIndex = 1;
            // 
            // btn_editProfile
            // 
            this.btn_editProfile.Location = new System.Drawing.Point(583, 554);
            this.btn_editProfile.Name = "btn_editProfile";
            this.btn_editProfile.Size = new System.Drawing.Size(123, 53);
            this.btn_editProfile.TabIndex = 5;
            this.btn_editProfile.Text = "Sửa";
            this.btn_editProfile.UseVisualStyleBackColor = true;
            this.btn_editProfile.Click += new System.EventHandler(this.btn_editProfile_Click);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManagerForm_FormClosing);
            this.tabCtrl_main.ResumeLayout(false);
            this.tab_account.ResumeLayout(false);
            this.tab_account.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_accountsList)).EndInit();
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
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_profileList)).EndInit();
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
        private System.Windows.Forms.DataGridView data_accountsList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.Label lb_userName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_role;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_idProfile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tb_passGenarator;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_resetPass;
        private System.Windows.Forms.Button btn_createAccount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_acceptAccount;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView data_profileList;
        private System.Windows.Forms.Button btn_editProfile;
    }
}