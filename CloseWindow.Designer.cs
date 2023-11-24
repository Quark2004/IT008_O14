namespace QLSV
{
	partial class CloseWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloseWindow));
			this.label2 = new System.Windows.Forms.Label();
			this.btn_Yes = new System.Windows.Forms.Button();
			this.btn_No = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Cambria", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(120, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(306, 33);
			this.label2.TabIndex = 1;
			this.label2.Text = "Bạn có muốn đăng xuất?";
			// 
			// btn_Yes
			// 
			this.btn_Yes.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_Yes.Location = new System.Drawing.Point(43, 136);
			this.btn_Yes.Name = "btn_Yes";
			this.btn_Yes.Size = new System.Drawing.Size(163, 57);
			this.btn_Yes.TabIndex = 2;
			this.btn_Yes.Text = "Đăng xuất";
			this.btn_Yes.UseVisualStyleBackColor = true;
			this.btn_Yes.Click += new System.EventHandler(this.btn_Yes_Click);
			// 
			// btn_No
			// 
			this.btn_No.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btn_No.Location = new System.Drawing.Point(232, 136);
			this.btn_No.Name = "btn_No";
			this.btn_No.Size = new System.Drawing.Size(163, 57);
			this.btn_No.TabIndex = 3;
			this.btn_No.Text = "Bỏ qua";
			this.btn_No.UseVisualStyleBackColor = true;
			this.btn_No.Click += new System.EventHandler(this.btn_No_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(14, 49);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 50);
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// CloseWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 218);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btn_No);
			this.Controls.Add(this.btn_Yes);
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CloseWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Đăng xuất";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btn_Yes;
		private System.Windows.Forms.Button btn_No;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}