using Microsoft.Win32;
using My_News.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
namespace My_News
{
    public partial class frmFirstRun : Form
    {
        string status = "";
        bool cmdEnable = true;
        bool user_select_no = false;
        public frmFirstRun()
        {
            InitializeComponent();
        }

        private void cmdCheckServer_Click(object sender, EventArgs e)
        {
            if (chkStartWithWindow.Checked == true)
            {
                Properties.Settings.Default.StartWithWindows = true;
                RegistryKey add = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                add.SetValue("My News", "\"" + Application.ExecutablePath.ToString() + "\"");
            }
            else
            {
                Properties.Settings.Default.StartWithWindows = false;
                RegistryKey remove = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                remove.DeleteValue("My News");
            }


            if (rdbWindow.Checked == true)
            {
                Properties.Settings.Default.db_server = txtAddressServer.Text;
                Properties.Settings.Default.db_username = txtId.Text;
                Properties.Settings.Default.db_password = txtPassword.Text;
                Properties.Settings.Default.db_use_trusted = true;

            }
            if (rdbWindow.Checked == false)
            {
                Properties.Settings.Default.db_server = txtAddressServer.Text;
                Properties.Settings.Default.db_username = txtId.Text;
                Properties.Settings.Default.db_password = txtPassword.Text;
                Properties.Settings.Default.db_use_trusted = false;
            }


            Properties.Settings.Default.Save();

            cmdEnable = false;

            Thread checkConnection = new Thread(new ThreadStart(CheckConnection));
            checkConnection.Start();

        }

        private void CheckConnection()
        {

            status = "Lưu cài đặt...";
            System.Threading.Thread.Sleep(500);


            status = "Đang kiểm tra kết nối SQL Server...";
            System.Threading.Thread.Sleep(1000);

            bool connect_ok = false;
            try
            {
                /////////////////////////////////////////////////////////////////
                // Import sample data.
                /////////////////////////////////////////////////////////////////

                string script = Properties.Resources.default_db;
                string connection_string;
                if (Properties.Settings.Default.db_use_trusted)
                {
                    connection_string = @"Server=" + Properties.Settings.Default.db_server + ";Trusted_Connection=True;";
                }
                else
                {
                    connection_string = @"Server=" + Properties.Settings.Default.db_server + ";User Id=" + Properties.Settings.Default.db_username + ";Password=" + Properties.Settings.Default.db_password + ";";
                }

                SqlConnection conn = new SqlConnection(connection_string);


                IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                           RegexOptions.Multiline | RegexOptions.IgnoreCase);

                conn.Open();
                connect_ok = true;
                status = "Đang nạp dữ liệu mẫu...";
                foreach (string commandString in commandStrings)
                {
                    if (commandString.Trim() != "")
                    {
                        new SqlCommand(commandString, conn).ExecuteNonQuery();
                    }
                }     
                conn.Close();

                System.Threading.Thread.Sleep(500);

                Properties.Settings.Default.FirstRun = 2;
                Properties.Settings.Default.Save();
                RestartMyself();

            }
            catch (Exception e)
            {
                if (connect_ok)
                {
                    status = "Không thể tạo Database!";
                    Properties.Settings.Default.FirstRun = 1;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Chú ý:\nĐể khởi tạo Database, chương trình cần chạy bằng quyền Admin\n\nNhấn OK và chọn Yes để có quyền Admin", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RestartMyself(true);
                }
                else
                {
                    status = "Kết nối thất bại! Vui lòng kiểm tra lại thông tin!";
                }
                Logger.Instance.Log("frmFirstRun.CheckConnection", e.ToString());
            }
            cmdEnable = true;
        }

        private void frmFirstRun_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }
        private void LoadSettings()
        {
            txtAddressServer.Text = Properties.Settings.Default.db_server;
            txtId.Text = Properties.Settings.Default.db_username;
            txtPassword.Text = Properties.Settings.Default.db_password;
            rdbSql.Checked = !Properties.Settings.Default.db_use_trusted;
            txtPassword.Enabled = !Properties.Settings.Default.db_use_trusted;
            txtId.Enabled = !Properties.Settings.Default.db_use_trusted;
            rdbWindow.Checked = Properties.Settings.Default.db_use_trusted;
        }
        private void RestartMyself(bool asAdmin = false)
        {
            try
            {
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
                Info.WindowStyle = ProcessWindowStyle.Hidden;
                Info.CreateNoWindow = true;
                Info.FileName = "cmd.exe";
                if (asAdmin)
                {
                    Info.UseShellExecute = true;
                    Info.Verb = "runas";
                }
                Process.Start(Info);
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception)
            {
                user_select_no = true;
            }
        }
        private void tmrText_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = status;
            cmdCheckServer.Visible = cmdEnable;
            picLoading.Visible = !cmdEnable;
            groupBox1.Enabled = cmdEnable;

            if (user_select_no)
            {
                tmrText.Stop();
                MessageBox.Show(this, "Bạn đã chọn NO, chương trình không thể tiếp tục.\nVui lòng chạy lại chương trình với quyền Admin", "!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void rdbSql_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSql.Checked)
            {
                txtId.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtId.Enabled = false;
                txtPassword.Enabled = false;
            }
        }

        private void frmFirstRun_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FirstRun == 1)
            {
                cmdCheckServer_Click(null, EventArgs.Empty);
            }
        }
    }
}
