namespace Quantrivienv._1
{
    partial class fAdmin
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txb_search = new System.Windows.Forms.TextBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.btn_Connectdata = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.txb_password = new System.Windows.Forms.TextBox();
            this.txb_username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Cambria", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabControl1.Location = new System.Drawing.Point(0, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1147, 712);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 36);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1139, 672);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Quản lí tài khoản";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_Exit);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txb_search);
            this.groupBox3.Controls.Add(this.btn_Search);
            this.groupBox3.Controls.Add(this.btn_Connectdata);
            this.groupBox3.Location = new System.Drawing.Point(0, 497);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1133, 159);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thao tác";
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(321, 36);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(118, 43);
            this.btn_Exit.TabIndex = 4;
            this.btn_Exit.Text = "Thoát";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(607, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 27);
            this.label3.TabIndex = 3;
            this.label3.Text = "(Username)";
            // 
            // txb_search
            // 
            this.txb_search.Location = new System.Drawing.Point(257, 99);
            this.txb_search.Name = "txb_search";
            this.txb_search.Size = new System.Drawing.Size(284, 34);
            this.txb_search.TabIndex = 2;
            this.txb_search.TextChanged += new System.EventHandler(this.txb_search_TextChanged);
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(8, 99);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(111, 41);
            this.btn_Search.TabIndex = 1;
            this.btn_Search.Text = "Tìm kiếm";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // btn_Connectdata
            // 
            this.btn_Connectdata.Location = new System.Drawing.Point(8, 36);
            this.btn_Connectdata.Name = "btn_Connectdata";
            this.btn_Connectdata.Size = new System.Drawing.Size(211, 43);
            this.btn_Connectdata.TabIndex = 0;
            this.btn_Connectdata.Text = "Kết nối Dữ liệu";
            this.btn_Connectdata.UseVisualStyleBackColor = true;
            this.btn_Connectdata.Click += new System.EventHandler(this.btn_Connectdata_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(-4, 247);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1137, 244);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách Tài khoản";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1137, 192);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Delete);
            this.groupBox1.Controls.Add(this.btn_Edit);
            this.groupBox1.Controls.Add(this.btn_Add);
            this.groupBox1.Controls.Add(this.txb_password);
            this.groupBox1.Controls.Add(this.txb_username);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(-4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1137, 238);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tài khoản ";
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(426, 193);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 39);
            this.btn_Delete.TabIndex = 6;
            this.btn_Delete.Text = "Xóa";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(284, 193);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(75, 39);
            this.btn_Edit.TabIndex = 5;
            this.btn_Edit.Text = "Sửa";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(119, 193);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(94, 39);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "Thêm ";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // txb_password
            // 
            this.txb_password.Location = new System.Drawing.Point(224, 86);
            this.txb_password.Name = "txb_password";
            this.txb_password.Size = new System.Drawing.Size(263, 34);
            this.txb_password.TabIndex = 3;
            // 
            // txb_username
            // 
            this.txb_username.Location = new System.Drawing.Point(224, 23);
            this.txb_username.Name = "txb_username";
            this.txb_username.Size = new System.Drawing.Size(263, 34);
            this.txb_username.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 36);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1139, 672);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Đăng kí học phần";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // fAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 736);
            this.Controls.Add(this.tabControl1);
            this.Name = "fAdmin";
            this.Text = "fQuanTriVien";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_Connectdata;
        private System.Windows.Forms.TextBox txb_password;
        private System.Windows.Forms.TextBox txb_username;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txb_search;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Exit;
    }
}