namespace QLSV
{
	partial class LoginForm
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
			this.lb_login = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.tb_userName = new System.Windows.Forms.TextBox();
			this.lb_userName = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tb_password = new System.Windows.Forms.TextBox();
			this.lb_password = new System.Windows.Forms.Label();
			this.btn_login = new System.Windows.Forms.Button();
			this.btn_exit = new System.Windows.Forms.Button();
			this.pn_login = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pic_login = new System.Windows.Forms.PictureBox();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.pn_login.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pic_login)).BeginInit();
			this.SuspendLayout();
			// 
			// lb_login
			// 
			this.lb_login.AutoSize = true;
			this.lb_login.BackColor = System.Drawing.Color.PaleTurquoise;
			this.lb_login.Font = new System.Drawing.Font("Cambria", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lb_login.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.lb_login.Location = new System.Drawing.Point(199, 63);
			this.lb_login.Name = "lb_login";
			this.lb_login.Size = new System.Drawing.Size(199, 38);
			this.lb_login.TabIndex = 0;
			this.lb_login.Text = "ĐĂNG NHẬP";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tb_userName);
			this.panel2.Controls.Add(this.lb_userName);
			this.panel2.Location = new System.Drawing.Point(27, 164);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(417, 96);
			this.panel2.TabIndex = 1;
			// 
			// tb_userName
			// 
			this.tb_userName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tb_userName.BackColor = System.Drawing.Color.White;
			this.tb_userName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_userName.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_userName.Location = new System.Drawing.Point(16, 42);
			this.tb_userName.MaxLength = 20;
			this.tb_userName.Name = "tb_userName";
			this.tb_userName.Size = new System.Drawing.Size(386, 39);
			this.tb_userName.TabIndex = 1;
			this.tb_userName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_userName_KeyDown);
			// 
			// lb_userName
			// 
			this.lb_userName.AutoSize = true;
			this.lb_userName.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lb_userName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lb_userName.Location = new System.Drawing.Point(11, 6);
			this.lb_userName.Name = "lb_userName";
			this.lb_userName.Size = new System.Drawing.Size(134, 23);
			this.lb_userName.TabIndex = 0;
			this.lb_userName.Text = "Tên đăng nhập";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.tb_password);
			this.panel3.Controls.Add(this.lb_password);
			this.panel3.Location = new System.Drawing.Point(27, 266);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(417, 96);
			this.panel3.TabIndex = 2;
			// 
			// tb_password
			// 
			this.tb_password.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.tb_password.BackColor = System.Drawing.Color.White;
			this.tb_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tb_password.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tb_password.Location = new System.Drawing.Point(16, 42);
			this.tb_password.MaxLength = 15;
			this.tb_password.Name = "tb_password";
			this.tb_password.Size = new System.Drawing.Size(386, 39);
			this.tb_password.TabIndex = 2;
			this.tb_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_password_KeyDown);
			// 
			// lb_password
			// 
			this.lb_password.AutoSize = true;
			this.lb_password.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lb_password.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lb_password.Location = new System.Drawing.Point(11, 6);
			this.lb_password.Name = "lb_password";
			this.lb_password.Size = new System.Drawing.Size(89, 23);
			this.lb_password.TabIndex = 0;
			this.lb_password.Text = "Mật khẩu";
			// 
			// btn_login
			// 
			this.btn_login.BackColor = System.Drawing.Color.DarkCyan;
			this.btn_login.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_login.ForeColor = System.Drawing.Color.Azure;
			this.btn_login.Location = new System.Drawing.Point(78, 396);
			this.btn_login.Name = "btn_login";
			this.btn_login.Size = new System.Drawing.Size(125, 59);
			this.btn_login.TabIndex = 3;
			this.btn_login.Text = "ĐĂNG NHẬP";
			this.btn_login.UseVisualStyleBackColor = false;
			this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
			this.btn_login.MouseHover += new System.EventHandler(this.btn_login_MouseHover);
			// 
			// btn_exit
			// 
			this.btn_exit.BackColor = System.Drawing.Color.DarkCyan;
			this.btn_exit.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_exit.ForeColor = System.Drawing.Color.Azure;
			this.btn_exit.Location = new System.Drawing.Point(271, 396);
			this.btn_exit.Name = "btn_exit";
			this.btn_exit.Size = new System.Drawing.Size(111, 59);
			this.btn_exit.TabIndex = 4;
			this.btn_exit.Text = "THOÁT";
			this.btn_exit.UseVisualStyleBackColor = false;
			this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
			this.btn_exit.MouseHover += new System.EventHandler(this.btn_exit_MouseHover);
			// 
			// pn_login
			// 
			this.pn_login.BackColor = System.Drawing.Color.PaleTurquoise;
			this.pn_login.Controls.Add(this.pictureBox1);
			this.pn_login.Controls.Add(this.btn_exit);
			this.pn_login.Controls.Add(this.btn_login);
			this.pn_login.Controls.Add(this.panel3);
			this.pn_login.Controls.Add(this.panel2);
			this.pn_login.Controls.Add(this.lb_login);
			this.pn_login.Location = new System.Drawing.Point(496, 0);
			this.pn_login.Name = "pn_login";
			this.pn_login.Size = new System.Drawing.Size(471, 501);
			this.pn_login.TabIndex = 1;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = global::QLSV.Properties.Resources.uit_logo;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pictureBox1.Location = new System.Drawing.Point(78, 33);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(115, 97);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// pic_login
			// 
			this.pic_login.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.pic_login.Image = global::QLSV.Properties.Resources.login_bg2;
			this.pic_login.Location = new System.Drawing.Point(0, -1);
			this.pic_login.Name = "pic_login";
			this.pic_login.Size = new System.Drawing.Size(496, 502);
			this.pic_login.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pic_login.TabIndex = 0;
			this.pic_login.TabStop = false;
			// 
			// LoginForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(966, 501);
			this.Controls.Add(this.pn_login);
			this.Controls.Add(this.pic_login);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Đăng Nhập";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.pn_login.ResumeLayout(false);
			this.pn_login.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pic_login)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pic_login;
		private System.Windows.Forms.Label lb_login;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox tb_userName;
		private System.Windows.Forms.Label lb_userName;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TextBox tb_password;
		private System.Windows.Forms.Label lb_password;
		private System.Windows.Forms.Button btn_login;
		private System.Windows.Forms.Button btn_exit;
		private System.Windows.Forms.Panel pn_login;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}

