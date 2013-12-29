using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using My_News.DAL;
using System.Collections;
using My_News.Window_Forms;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
namespace My_News
{
    public partial class frmMain : Form
    {
        public static frmMain Instance;

        private List<CaSo> casomenu = new List<CaSo>();
        private List<CaSo> casoselected = new List<CaSo>();

        public static frmMain GetInstance()
        {
            if (frmMain.Instance == null)
                frmMain.Instance = new frmMain();

            return frmMain.Instance;
        }

        public frmMain()
        {
            InitializeComponent();

        }

        private void LoadInfo()
        {
            List<Source> list = new List<Source>();
            list = (new SourceDAL()).GetAllSource();

            if (list != null)
            {

                int sourceCount = list.Count;
                lblSource.Text = sourceCount.ToString();

                List<Category> listCategory = new List<Category>();
                listCategory = (new CategoryDAL()).GetAllCategory();
                int categoryCount = listCategory.Count;
                lblSelected.Text = categoryCount.ToString();

                lblLastUpdate.Text = Properties.Settings.Default.LastUpdate.ToString();

                List<Rsslink> listRsslink = new List<Rsslink>();
                listRsslink = (new RsslinkDAL()).GetAllRsslink();
                int rssCount = listRsslink.Count;
                lblLinkRss.Text = listRsslink.Count.ToString();
            }

        }
        private void LoadSettings()
        {
            nudTime.Value = Properties.Settings.Default.SyncDelay;
            nudSpeed.Value = Properties.Settings.Default.TextSpeed;
            chkJustShowTitle.Checked = Properties.Settings.Default.TitleOnly;
            chkStartWithWindow.Checked = Properties.Settings.Default.StartWithWindows;
            tkbOpacity.Value = Properties.Settings.Default.TextOpacity;
            txtAddressServer.Text = Properties.Settings.Default.db_server;
            txtId.Text = Properties.Settings.Default.db_username;
            txtPassword.Text = Properties.Settings.Default.db_password;
            rdbSql.Checked = !Properties.Settings.Default.db_use_trusted;
            txtPassword.Enabled = !Properties.Settings.Default.db_use_trusted;
            txtId.Enabled = !Properties.Settings.Default.db_use_trusted;
            rdbWindow.Checked = Properties.Settings.Default.db_use_trusted;
            txtFont.Font = Properties.Settings.Default.TextFont;
            fontDialog1.Font = Properties.Settings.Default.TextFont;
            nudNewsDays.Value = Properties.Settings.Default.NewsDays;
            if (chkStartWithWindow.Checked == true)
            {
                RegistryKey add = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                add.SetValue("My News", "\"" + Application.ExecutablePath.ToString() + "\"");
            }
            else
            {
                RegistryKey remove = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                remove.DeleteValue("My News");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadSettings();
            if (!DbHelper.NoServerMode)
            {
                BasicLoadData();
            }
            else
            {
                tctMain.SelectTab("tbSettings");
            }
        }
        private void ShowGuide()
        {
            MessageBox.Show(this, "Chào mừng bạn đến với My News - Thế giới trong tầm mắt\n\n" +
                                    "- Tùy chọn nội dung của bạn ở thẻ\"Chọn nguồn tin\"\n" +
                                    "- Nhấn giữ nút Ctrl và dùng chuột để xem tin tức của bạn.\n\n" +
                                    "Cảm ơn bạn đã sử dụng chương trình!", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
        }
        private void BasicLoadData()
        {
            LoadAllData();
            LoadInfo();
            LoadSourceName("");
            LoadCategoryName("");
            LoadSelectedSourceAndCategory();
            LoadSourceAndCategory();
        }
        private void LoadAllData()
        {
            List<Rsslink> list = new List<Rsslink>();
            List<DataLoader> loadlist = new List<DataLoader>();
            list = (new RsslinkDAL()).GetAllRsslink();
            if (list != null)
            {
                foreach (var item in list)
                {
                    DataLoader load = new DataLoader();
                    load.Id = item.Id;
                    load.Url = item.Url.TrimEnd(' ');
                    load.SourceName = item.GetSource.Name;
                    load.CategoryName = item.GetCategory.Name;
                    loadlist.Add(load);
                }
                dgdData.AutoGenerateColumns = false;
                dgdData.DataSource = loadlist;
            }
            else
            {
                MessageBox.Show("Không thể kết nối đến Server!\nVui lòng kiểm tra lại cài đặt:\n" + DbHelper.GetConnectionString(), "Lỗi kết nối!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadSourceName(string sou)
        {
            List<Source> list = new List<Source>();
            list = (new SourceDAL()).GetAllSource();
            List<String> liststr = new List<string>();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        liststr.Add(item.Name);
                    }
                }
                else
                {
                    liststr.Add("");
                }
                liststr.Add("[Quản lý]");
                cboSource.DataSource = liststr;
                cboSource.Text = sou;
            }
        }

        private void LoadCategoryName(string cat)
        {
            List<Category> list = new List<Category>();
            list = (new CategoryDAL()).GetAllCategory();
            List<String> liststr = new List<string>();
            if (list != null)
            {
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        liststr.Add(item.Name);
                    }
                }
                else
                {
                    liststr.Add("");
                }
                liststr.Add("[Quản lý]");
                cboCategory.DataSource = liststr;
                cboCategory.Text = cat;
            }
        }

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            if (txtRss.Text.Trim() != "")
            {
                Source sou = (new SourceDAL()).GetSourceByName(cboSource.Text);
                Category cat = (new CategoryDAL()).GetCategoryByName(cboCategory.Text);
                bool rs = (new RsslinkDAL()).InsertRsslink(new Rsslink(0, txtRss.Text, cat, sou));
                if (rs)
                {
                    MessageBox.Show("Thêm mới Rss Link thành công.");
                    LoadAllData();
                    LoadSourceAndCategory();
                }
                else
                {
                    MessageBox.Show("Thêm mới Rss Link thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                }
            }
            else
            {
                MessageBox.Show("Thêm mới Rss Link thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
            }
        }

        private void dgdData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtRss.Text = (string)dgdData.SelectedRows[0].Cells[3].Value;
            cboSource.Text = (string)dgdData.SelectedRows[0].Cells[1].Value;
            cboCategory.Text = (string)dgdData.SelectedRows[0].Cells[2].Value;
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa Rss Link này không?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int id = (int)dgdData.SelectedRows[0].Cells[0].Value;
                bool rs = (new RsslinkDAL()).DeleteRsslink(id);

                if (rs)
                {
                    LoadAllData();
                    LoadSourceAndCategory();
                }
                else
                {
                    MessageBox.Show("Xóa Rss Link thất bại. Vui lòng kiểm tra kết nối tới server.");
                }
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (txtRss.Text.Trim() != "")
            {
                Source sou = (new SourceDAL()).GetSourceByName(cboSource.Text);
                Category cat = (new CategoryDAL()).GetCategoryByName(cboCategory.Text);
                int id = (int)dgdData.SelectedRows[0].Cells[0].Value;
                bool rs = (new RsslinkDAL()).UpdateRsslink(new Rsslink(id, txtRss.Text, cat, sou));
                if (rs)
                {
                    MessageBox.Show("Cập nhật Rss Link thành công.");
                    LoadAllData();
                    LoadSourceAndCategory();
                }
                else
                {
                    MessageBox.Show("Cập nhật Rss Link thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
                }
            }
            else
            {
                MessageBox.Show("Cập nhật Rss Link thất bại. Vui lòng kiểm tra:\n    - Tên chuyên mục bị trùng.\n    - Nhập đầy đủ các thông tin.\n    - Kết nối tới server.");
            }
            //IComparer<DataLoader> myComparer = new myReverserClass();
            //loadlist.Sort(myComparer);
            //dgdData.AutoGenerateColumns = false;
            //dgdData.DataSource = loadlist;
            //dgdData.Refresh();

        }

        private void dgdData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void rdbSql_Click(object sender, EventArgs e)
        {
            txtAddressServer.Enabled = true;
            txtId.Enabled = true;
            txtPassword.Enabled = true;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdSave_Click(object sender, EventArgs e)
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

            if (chkJustShowTitle.Checked == true)
            {
                Properties.Settings.Default.TitleOnly = true;
            }
            else
            {
                Properties.Settings.Default.TitleOnly = false;
            }

            Properties.Settings.Default.SyncDelay = Convert.ToInt32(nudTime.Value);
            Properties.Settings.Default.TextSpeed = Convert.ToInt32(nudSpeed.Value);
            Properties.Settings.Default.TextOpacity = Convert.ToInt32(tkbOpacity.Value);
            Properties.Settings.Default.NewsDays = Convert.ToInt32(nudNewsDays.Value);
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

            Properties.Settings.Default.TextFont = fontDialog1.Font;


            Properties.Settings.Default.Save();
            MessageBox.Show("Lưu cài đặt thành công!");

            //Reconnect server if current is off
            if (DbHelper.NoServerMode)
            {
                if (DbHelper.IsConnected())
                {
                    MessageBox.Show("Kết nối server thành công!");
                    DbHelper.NoServerMode = false;
                    Thread autoSyncThread = new Thread(new ThreadStart(mnRssSync.Instance.Run));
                    autoSyncThread.Start();
                    BasicLoadData();
                }
                else
                {
                    MessageBox.Show(String.Format("Không tìm thấy server:\n{0}\nVui lòng kiểm tra lại kết nối!", DbHelper.GetConnectionString()), "Lỗi kết nối!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void rdbWindow_Click(object sender, EventArgs e)
        {

            txtId.Enabled = false;
            txtPassword.Enabled = false;
        }

        private void cmdDefault_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartWithWindows = true;
            chkStartWithWindow.Checked = true;
            Properties.Settings.Default.TitleOnly = true;
            chkJustShowTitle.Checked = true;
            Properties.Settings.Default.TextSpeed = 50;
            nudSpeed.Value = 50;
            Properties.Settings.Default.SyncDelay = 15;
            nudTime.Value = 15;
            Properties.Settings.Default.TextOpacity = 50;
            tkbOpacity.Value = 50;
            txtAddressServer.Text = "";
            txtId.Text = "";
            txtPassword.Text = "";
            rdbWindow.Checked = true;
            fontDialog1.Font = this.Font;
            txtFont.Font = this.Font;
        }

        private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSource.Text == "[Quản lý]")
            {
                frmNewSource frmSou = new frmNewSource();
                frmSou.NotifyEvent += new NotifyLoadData(ReloadData);
                frmSou.NotifyEvent += new NotifyLoadData(LoadSourceName);
                LoadSourceName("");
                frmSou.ShowDialog();

            }
        }

        private void ReloadData(string str)
        {
            BasicLoadData();
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCategory.Text == "[Quản lý]")
            {
                frmNewCategory frmCat = new frmNewCategory();
                frmCat.NotifyEvent += new NotifyLoadData(ReloadData);
                frmCat.NotifyEvent += new NotifyLoadData(LoadCategoryName);
                LoadCategoryName("");
                frmCat.ShowDialog();

            }
        }


        private void txtFont_Click(object sender, EventArgs e)
        {
            DialogResult result = fontDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Get Font.
                Font font = fontDialog1.Font;
                //// Set TextBox properties.

                this.txtFont.Font = font;
            }
        }
        private void LoadCasomenuToListbox()
        {
            List<string> liststr = new List<string>();
            foreach (var item in casomenu)
            {
                string str = item.Source.Name + "  -  " + item.Category.Name;
                liststr.Add(str);
            }

            lstMenu.DataSource = liststr;
        }
        private void LoadSourceAndCategory()
        {
            try
            {
                casomenu = (new CaSoDAL()).GetAllCaSo();

                // Remove selected CaSo
                foreach (CaSo c in casoselected)
                {
                    for (int i = casomenu.Count - 1; i >= 0; --i)
                    {
                        if ((casomenu[i].Source.Name + "  -  " + casomenu[i].Category.Name).Equals(c.Source.Name + "  -  " + c.Category.Name))
                        {
                            casomenu.RemoveAt(i);
                        }
                    }
                }

                LoadCasomenuToListbox();
            }
            catch (Exception)
            {
                MessageBox.Show("Load data thất bại.");
            }
        }
        private void LoadCasoselectedToListbox()
        {
            List<string> liststr = new List<string>();
            foreach (var item in casoselected)
            {
                string str = item.Source.Name + "  -  " + item.Category.Name;
                liststr.Add(str);
            }

            lstSelected.DataSource = liststr;
        }
        private void LoadSelectedSourceAndCategory()
        {
            try
            {
                casoselected = (new CaSoDAL()).GetUserSelectedCaSo();
                LoadCasoselectedToListbox();
            }
            catch (Exception)
            {
                MessageBox.Show("Load data thất bại.");
                //log
            }
        }

        private void cmdMinimize_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            mnRssSync.Instance.Stop();
            frmMyNews.Instance.Dispose();
        }

        private int GetIndexByString(string str, List<CaSo> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (str.Equals(list[i].Source.Name + "  -  " + list[i].Category.Name))
                {
                    return i;
                }
            }

            return -1;
        }
        private void cmdGet_Click(object sender, EventArgs e)
        {
            if (lstMenu.SelectedItems.Count > 0)
            {
                //for (int i = lstMenu.Items.Count - 1; i >= 0; --i)
                foreach (var item in lstMenu.SelectedItems)
                {
                    int target = GetIndexByString(item.ToString(), casomenu);
                    casoselected.Add(casomenu[target]);
                    casomenu.RemoveAt(target);
                    //casoselected.Add(casomenu[GetIndexByString(lstMenu.Items[i].ToString(), casomenu)]);

                }
            }
            LoadCasoselectedToListbox();
            LoadCasomenuToListbox();
        }

        private void cmdGetAll_Click(object sender, EventArgs e)
        {
            foreach (CaSo c in casomenu)
            {
                casoselected.Add(c);
            }
            casomenu.Clear();

            LoadCasoselectedToListbox();
            LoadCasomenuToListbox();
        }

        private void cmdGetBack_Click(object sender, EventArgs e)
        {
            if (lstSelected.SelectedItems.Count > 0)
            {
                //for (int i = lstMenu.Items.Count - 1; i >= 0; --i)
                foreach (var item in lstSelected.SelectedItems)
                {
                    int target = GetIndexByString(item.ToString(), casoselected);
                    casomenu.Add(casoselected[target]);
                    casoselected.RemoveAt(target);
                    //casoselected.Add(casomenu[GetIndexByString(lstMenu.Items[i].ToString(), casomenu)]);

                }
            }
            LoadCasoselectedToListbox();
            LoadCasomenuToListbox();
        }

        private void cmdGetBackAll_Click(object sender, EventArgs e)
        {
            foreach (CaSo c in casoselected)
            {
                casomenu.Add(c);
            }
            casoselected.Clear();
            LoadCasoselectedToListbox();
            LoadCasomenuToListbox();
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            bool rs = (new CaSoDAL()).SetUserSelectedCaSo(casoselected);
            if (rs)
            {
                mnRssSync.Instance.StartSync();
                frmMyNews.Instance.ResetNews();
                MessageBox.Show("Cập nhật thành công");
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            LoadSelectedSourceAndCategory();
            LoadSourceAndCategory();
        }

        private void cmdSortAsCategory_Click(object sender, EventArgs e)
        {

            casomenu.Sort();
            List<string> liststr = new List<string>();
            foreach (var item in casomenu)
            {
                string str = item.Source.Name + "  -  " + item.Category.Name;
                liststr.Add(str);
            }

            lstMenu.DataSource = liststr;

        }

        private void cmdSoftAsSource_Click(object sender, EventArgs e)
        {
            IComparer<CaSo> compare = new myReverserClass();
            casomenu.Sort(compare);
            List<string> liststr = new List<string>();
            foreach (var item in casomenu)
            {
                string str = item.Source.Name + "  -  " + item.Category.Name;
                liststr.Add(str);
            }

            lstMenu.DataSource = liststr;
        }


        private void tctMain_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tctMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DbHelper.NoServerMode)
            {
                LoadInfo();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            frmMyNews.Instance.ResetNews();
        }

        private void label20_DoubleClick(object sender, EventArgs e)
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);
            label20.ForeColor = randomColor;
        }


        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void btnShowGuide_Click(object sender, EventArgs e)
        {
            ShowGuide();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FirstRun == 2)
            {
                Properties.Settings.Default.FirstRun = 3;
                Properties.Settings.Default.Save();
                ShowGuide();
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Bạn thực sực muốn gỡ bỏ chương trình này ra khỏi máy tính?", @":-(", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (d == System.Windows.Forms.DialogResult.Yes)
            {
                Properties.Settings.Default.FirstRun = 0;
                Properties.Settings.Default.Save();
                RegistryKey remove = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                remove.DeleteValue("My News");
                MessageBox.Show("Gỡ bỏ thành công!\n\nBất cứ lúc nào bạn muốn cài đặt lại chương trình, chỉ cần chạy lại file My News.exe", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mnRssSync.Instance.Stop();
                frmMyNews.Instance.Dispose();
            }
        }

        private void label18_DoubleClick(object sender, EventArgs e)
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);
            label18.ForeColor = randomColor;
        }












    }

    public class DataLoader
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string SourceName { get; set; }
        public string CategoryName { get; set; }
    }

    public class myReverserClass : IComparer<CaSo>
    {
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed. 
        int IComparer<CaSo>.Compare(CaSo x, CaSo y)
        {
            return x.Source.Name.CompareTo(y.Source.Name);
        }
    }
}
