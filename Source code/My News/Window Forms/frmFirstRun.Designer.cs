namespace My_News
{
    partial class frmFirstRun
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFirstRun));
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbSql = new System.Windows.Forms.RadioButton();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkStartWithWindow = new System.Windows.Forms.CheckBox();
            this.txtAddressServer = new System.Windows.Forms.TextBox();
            this.rdbWindow = new System.Windows.Forms.RadioButton();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cmdCheckServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tmrText = new System.Windows.Forms.Timer(this.components);
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(96, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(119, 29);
            this.label18.TabIndex = 10;
            this.label18.Text = "My News";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(82, 38);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(144, 16);
            this.label19.TabIndex = 11;
            this.label19.Text = "Thế giới trong tầm mắt!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Cài đặt cho lần chạy đầu tiên";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbSql);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.chkStartWithWindow);
            this.groupBox1.Controls.Add(this.txtAddressServer);
            this.groupBox1.Controls.Add(this.rdbWindow);
            this.groupBox1.Controls.Add(this.txtId);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(12, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 192);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cài đặt server";
            // 
            // rdbSql
            // 
            this.rdbSql.AutoSize = true;
            this.rdbSql.Location = new System.Drawing.Point(12, 70);
            this.rdbSql.Name = "rdbSql";
            this.rdbSql.Size = new System.Drawing.Size(154, 17);
            this.rdbSql.TabIndex = 8;
            this.rdbSql.Text = "Dùng tài khoản SQL server";
            this.rdbSql.UseVisualStyleBackColor = true;
            this.rdbSql.CheckedChanged += new System.EventHandler(this.rdbSql_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(86, 127);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(185, 20);
            this.txtPassword.TabIndex = 6;
            // 
            // chkStartWithWindow
            // 
            this.chkStartWithWindow.AutoSize = true;
            this.chkStartWithWindow.Checked = true;
            this.chkStartWithWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartWithWindow.Location = new System.Drawing.Point(9, 165);
            this.chkStartWithWindow.Name = "chkStartWithWindow";
            this.chkStartWithWindow.Size = new System.Drawing.Size(188, 17);
            this.chkStartWithWindow.TabIndex = 15;
            this.chkStartWithWindow.Text = "Khởi động My News cùng window";
            this.chkStartWithWindow.UseVisualStyleBackColor = true;
            // 
            // txtAddressServer
            // 
            this.txtAddressServer.Location = new System.Drawing.Point(89, 25);
            this.txtAddressServer.Name = "txtAddressServer";
            this.txtAddressServer.Size = new System.Drawing.Size(182, 20);
            this.txtAddressServer.TabIndex = 4;
            // 
            // rdbWindow
            // 
            this.rdbWindow.AutoSize = true;
            this.rdbWindow.Checked = true;
            this.rdbWindow.Location = new System.Drawing.Point(12, 51);
            this.rdbWindow.Name = "rdbWindow";
            this.rdbWindow.Size = new System.Drawing.Size(145, 17);
            this.rdbWindow.TabIndex = 7;
            this.rdbWindow.TabStop = true;
            this.rdbWindow.Text = "Dùng tài khoản Windows";
            this.rdbWindow.UseVisualStyleBackColor = true;
            // 
            // txtId
            // 
            this.txtId.Enabled = false;
            this.txtId.Location = new System.Drawing.Point(86, 96);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(185, 20);
            this.txtId.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Địa chỉ Server :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Tài khoản :";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 130);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Mật khẩu :";
            // 
            // cmdCheckServer
            // 
            this.cmdCheckServer.Location = new System.Drawing.Point(85, 331);
            this.cmdCheckServer.Name = "cmdCheckServer";
            this.cmdCheckServer.Size = new System.Drawing.Size(141, 26);
            this.cmdCheckServer.TabIndex = 14;
            this.cmdCheckServer.Text = "Kiểm tra kết nối";
            this.cmdCheckServer.UseVisualStyleBackColor = true;
            this.cmdCheckServer.Click += new System.EventHandler(this.cmdCheckServer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(12, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "*Chương trình yêu cầu SQL Server để chạy";
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblStatus.Location = new System.Drawing.Point(12, 301);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(284, 27);
            this.lblStatus.TabIndex = 17;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrText
            // 
            this.tmrText.Enabled = true;
            this.tmrText.Interval = 10;
            this.tmrText.Tick += new System.EventHandler(this.tmrText_Tick);
            // 
            // picLoading
            // 
            this.picLoading.Image = global::My_News.Properties.Resources.loading;
            this.picLoading.Location = new System.Drawing.Point(130, 331);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(48, 26);
            this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLoading.TabIndex = 18;
            this.picLoading.TabStop = false;
            this.picLoading.Visible = false;
            // 
            // frmFirstRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 391);
            this.Controls.Add(this.picLoading);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdCheckServer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmFirstRun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cài đặt server";
            this.Load += new System.EventHandler(this.frmFirstRun_Load);
            this.Shown += new System.EventHandler(this.frmFirstRun_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbSql;
        private System.Windows.Forms.RadioButton rdbWindow;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAddressServer;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button cmdCheckServer;
        private System.Windows.Forms.CheckBox chkStartWithWindow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer tmrText;
        private System.Windows.Forms.PictureBox picLoading;
    }
}