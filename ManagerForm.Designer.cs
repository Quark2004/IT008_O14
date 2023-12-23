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
			this.tabCtrl_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtrl_main
			// 
			this.tabCtrl_main.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabCtrl_main.Controls.Add(this.tab_account);
			this.tabCtrl_main.Controls.Add(this.tab_course);
			this.tabCtrl_main.Controls.Add(this.tab_registration);
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
			this.tab_registration.Location = new System.Drawing.Point(4, 38);
			this.tab_registration.Name = "tab_registration";
			this.tab_registration.Padding = new System.Windows.Forms.Padding(3);
			this.tab_registration.Size = new System.Drawing.Size(1359, 617);
			this.tab_registration.TabIndex = 2;
			this.tab_registration.Text = "   ĐKHP";
			this.tab_registration.UseVisualStyleBackColor = true;
			// 
			// ManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1369, 657);
			this.Controls.Add(this.tabCtrl_main);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Quản lý";
			this.tabCtrl_main.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrl_main;
		private System.Windows.Forms.TabPage tab_account;
		private System.Windows.Forms.TabPage tab_course;
		private System.Windows.Forms.TabPage tab_registration;
	}
}