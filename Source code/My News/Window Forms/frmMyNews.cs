using My_News.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace My_News
{
    public partial class frmMyNews : Form
    {

        public static frmMyNews Instance;
        /// ////////////////////////////////////////////////////////////////////
        /// Needs for set Top Most
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(HandleRef hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        /// ///////////////////////////////////////////////////////////////////////

        private List<Item> itemList = new List<Item>();
        private int step = 1;
        private int mouseHoverRange = 100;
        private List<LinkLabel> listLl = new List<LinkLabel>();

        private int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        private int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

        private List<Showing_Label> showing_labels = new List<Showing_Label>();
        /// //////////////////////////////////////////////////////////////////////
        public static frmMyNews GetInstance()
        {
            if (frmMyNews.Instance == null)
                frmMyNews.Instance = new frmMyNews();

            return frmMyNews.Instance;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        public frmMyNews()
        {
            InitializeComponent();
            SetTopMostForm();
        }

        private void SetTopMostForm()
        {
            // force window to have focus
            uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            uint appThread = GetCurrentThreadId();
            const uint SW_SHOW = 5;
            if (foreThread != appThread)
            {
                AttachThreadInput(foreThread, appThread, true);
                BringWindowToTop(this.Handle);
                ShowWindow(this.Handle, SW_SHOW);
                AttachThreadInput(foreThread, appThread, false);
            }
            else
            {
                BringWindowToTop(this.Handle);
                ShowWindow(this.Handle, SW_SHOW);
            }
            this.Activate();
        }
        public void Run(List<Item> list)
        {
            itemList = list;
        }
        private void tmrText_Tick(object sender, EventArgs e)
        {
            /*
             * Nếu đuôi của showing_label[cuối] < screenWidth thì thêm [label tiếp theo] vào showing_label
             * Dịch chuyển TẤT CẢ TRONG showing_label
             * Nếu đuôi của showing_label[đầu] < 0 thì remove label đầu ra khỏi showing_label.
             * 
             * */



            //if (pnlLinks.Left + pnlLinks.Width < 0) pnlLinks.Left = this.Width;
            //pnlLinks.Left -= step * 5;

            //Dịch chuyển TẤT CẢ TRONG showing_label
            foreach (var i in showing_labels)
            {
                listLl[i.LabelId].Left -= step;
            }
            //Nếu đuôi của showing_label[cuối] < screenWidth thì thêm [label tiếp theo] vào showing_label




            if (showing_labels[showing_labels.Count - 1].LabelId + 1 < listLl.Count)
            {
                if (showing_labels[showing_labels.Count - 1].Flag == 0 && listLl[showing_labels[showing_labels.Count - 1].LabelId].Left + listLl[showing_labels[showing_labels.Count - 1].LabelId].Width < screenWidth)
                {
                    showing_labels[showing_labels.Count - 1].Flag = 1;
                    showing_labels.Add(new Showing_Label(showing_labels[showing_labels.Count - 1].LabelId + 1, 0));
                }
            }
            //Nếu đuôi của showing_label[đầu] < 0 thì remove label đầu ra khỏi showing_label.
            if (showing_labels[0].Flag == 1 && listLl[showing_labels[0].LabelId].Left + listLl[showing_labels[0].LabelId].Width < 0)
            {
                showing_labels[0].Flag = 2;
                showing_labels.RemoveAt(0);
                if (showing_labels[0].LabelId + 1 == listLl.Count)
                {
                    //MessageBox.Show("ASD");
                    SetLabelsPosition();
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                frmMain.Instance.Show();
                frmMain.Instance.Activate();
            }
        }

        private void frmMyNews_Load(object sender, EventArgs e)
        {

            if (Properties.Settings.Default.FirstRun == 2)
            {
                frmMain.Instance.Show();
            }
            Properties.Settings.Default.PropertyChanged += ReloadSettings;
            mnRssSync.Instance.RssSyncDoneEvent += ResetNews;
            if (!DbHelper.NoServerMode)
            {
                ResetNews();
            }
            else
            {
                MessageBox.Show(String.Format("Không tìm thấy server:\n{0}\nVui lòng kiểm tra lại kết nối!", DbHelper.GetConnectionString()), "Lỗi kết nối!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                (new frmMain()).Show();
            }
        }

        //private delegate void dlgResetNews();
        public void ResetNews()
        {
            //CHeck for Cross-thread warning
            //if (this.pnlLinks.InvokeRequired)
            //{
            //    this.Invoke(new dlgResetNews(ResetNews));
            //    return;
            //}
            RefreshItems();
            if (itemList != null)
            {
                if (itemList.Count > 0)
                {
                    SetAllItemString();
                    LoadTextSettings();

                    SetLabelsPosition();

                    //pnlLinks.Left = this.Width;
                }
            }
        }

        private void ReloadSettings(object sender, PropertyChangedEventArgs e)
        {
            LoadTextSettings();
        }

        private delegate void dlgLoadTextSettings();
        private void LoadTextSettings()
        {
            try
            {
                //CHeck for Cross-thread warning
                if (this.InvokeRequired)
                {
                    this.Invoke(new dlgLoadTextSettings(LoadTextSettings));
                    return;
                }

                foreach (LinkLabel ll in listLl)
                {
                    ll.Font = Properties.Settings.Default.TextFont;
                }

                int savedSpeed = Properties.Settings.Default.TextSpeed;
                if (savedSpeed < 60)
                {
                    step = 1;
                    tmrText.Interval = (61 - savedSpeed);
                }
                else
                {
                    step = 2;
                    tmrText.Interval = 101 - savedSpeed;
                }

                this.Opacity = Properties.Settings.Default.TextOpacity * 1.0 / 100;

                SetLabelsPosition();

                //////////////////////////////////
                if (listLl.Count > 0)
                {
                    //pnlLinks.Height = listLl[0].Height;
                    //pnlLinks.Width = listLl[listLl.Count - 1].Left + listLl[listLl.Count - 1].Width;
                    this.Height = listLl[0].Height;
                }
                else
                {
                    //pnlLinks.Height = 0;
                    this.Height = 0;
                }

                this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.X;
                this.Width = screenWidth;
                //this.Height = pnlLinks.Height;
                this.Top = screenHeight - this.Height;
                tmrMouseTracker.Start();
                tmrText.Start();

                //pnlLinks.Top = 0;

            }
            catch (Exception e)
            {
                Logger.Instance.Log("frmMyNews.LoadTextSettings", e.ToString());
            }
        }

        private void SetLabelsPosition()
        {
            for (int i = 1; i < listLl.Count; ++i)
            {
                listLl[i].Left = screenWidth;
            }
            showing_labels.Clear();
            showing_labels.Add(new Showing_Label());
            //MessageBox.Show("ASD");
        }

        private delegate void dlgSetAllItemString();
        private void SetAllItemString() //Just for demo
        {
            //Check for Cross-Thread warining
            if (this.InvokeRequired)
            {
                this.Invoke(new dlgSetAllItemString(SetAllItemString));
                return;
            }

            //Remove all exists linklabel:
            foreach (LinkLabel l in listLl)
            {
                this.Controls.Remove(l);
                l.Dispose();
            }

            listLl = new List<LinkLabel>();

            //int counter = 0;
            foreach (Item item in itemList)
            {

                LinkLabel ll = new LinkLabel();
                //pnlLinks.Controls.Add(ll);
                this.Controls.Add(ll);
                ll.AutoSize = true;
                ll.Name = "item_label" + item.Id;
                ll.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
                ll.LinkColor = System.Drawing.Color.Black;
                ll.Text = " ●   " + item.Title;
                ll.Font = Properties.Settings.Default.TextFont;
                //if (counter == 0)
                //{
                //    ll.Location = new System.Drawing.Point(0, 0);
                //}
                //else
                //{
                //    LinkLabel _ll = listLl[counter - 1];
                //    ll.Location = new System.Drawing.Point(_ll.Left + _ll.Width, 0);
                //}
                ll.Location = new System.Drawing.Point(screenWidth, 0);
                toolTip1.SetToolTip(ll, item.Title + Environment.NewLine + Environment.NewLine + (new StrHelper()).HtmlToPlainText(item.Description));
                LinkLabelLinkClickedEventHandler handler = delegate(Object o, LinkLabelLinkClickedEventArgs eX)
                {
                    if (eX.Button == MouseButtons.Left)
                    {
                        Process.Start(new ProcessStartInfo("explorer.exe", item.Link.Trim()));
                    }
                };
                ll.LinkClicked += handler;
                listLl.Add(ll);

                //counter++;

            }

        }

        private void RefreshItems()
        {

            #region LoadItemsCase1
            ////////////////// Get Item for Case 1: download all rsslink, view selected only

            //List<CaSo> listCaSo = new List<CaSo>();
            //listCaSo = (new CaSoDAL()).GetUserSelectedCaSo();

            //List<Rsslink> listRsslink = new List<Rsslink>();

            //foreach (CaSo c in listCaSo)
            //{
            //    List<Rsslink> l = new List<Rsslink>();
            //    l = (new RsslinkDAL()).GetRsslinksByCaSo(c);
            //    foreach (Rsslink rl in l)
            //    {
            //        listRsslink.Add(rl);
            //    }
            //}


            //itemList = new List<Item>();

            //foreach (Rsslink rsslink in listRsslink)
            //{
            //    List<Item> _item = new List<Item>();
            //    _item = (new ItemDAL()).GetItemsByRsslink(rsslink.Id);

            //    foreach (Item i in _item)
            //    {
            //        itemList.Add(i);
            //    }
            //}
            #endregion

            #region LoadItemsCase2
            itemList = new List<Item>();
            itemList = (new ItemDAL()).GetAllItem();
            #endregion
        }

        private void tmrMouseTracker_Tick(object sender, EventArgs e)
        {

            if (Control.ModifierKeys == Keys.Control && Cursor.Position.Y > this.Top && Cursor.Position.X < System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 10)
            {
                step = -4;
                tmrText.Interval = 1;
            }
            else
            {
                step = 1;
                double A = Properties.Settings.Default.TextOpacity * 1.0 / 100;
                if (Cursor.Position.Y > this.Top - mouseHoverRange)
                {
                    double percentDistance = (this.Top - Cursor.Position.Y) * 1.0 / mouseHoverRange;
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        this.Opacity = A + (1 - A) * (1 - percentDistance);
                    }
                    else
                    {
                        this.Opacity = A * percentDistance;
                    }

                    int B;
                    if (Properties.Settings.Default.TextSpeed < 60)
                    {
                        B = (61 - Properties.Settings.Default.TextSpeed);
                    }
                    else
                    {
                        B = 101 - Properties.Settings.Default.TextSpeed;
                    }
                    //int newI = 101 - Convert.ToInt32(Math.Min(100 - B, 100 * percentDistance));
                    int newI = B + Convert.ToInt32((100 - B) * (1 - percentDistance));
                    if (newI > 0)
                    {
                        tmrText.Enabled = true;
                        tmrText.Interval = newI;
                    }
                    else
                    {
                        tmrText.Enabled = false;
                    }
                }
                else
                {
                    this.Opacity = Properties.Settings.Default.TextOpacity * 1.0 / 100;
                    int savedSpeed = Properties.Settings.Default.TextSpeed;
                    tmrText.Enabled = true;
                    if (savedSpeed < 60)
                    {
                        step = 1;
                        tmrText.Interval = (61 - savedSpeed);
                    }
                    else
                    {
                        step = 2;
                        tmrText.Interval = 101 - savedSpeed;
                    }
                }
            }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            mnRssSync.Instance.Stop();
            frmMyNews.Instance.Dispose();
        }

        private void mnuSetup_Click(object sender, EventArgs e)
        {
            frmMain.Instance.Show();
            frmMain.Instance.Activate();
        }



    }

    class Showing_Label
    {
        public int LabelId { get; set; }
        public int Flag { get; set; }
        public Showing_Label()
        {
            LabelId = 0;
            Flag = 0;
        }
        public Showing_Label(int labelId, int flag)
        {
            LabelId = labelId;
            Flag = flag;
        }
    }
}
