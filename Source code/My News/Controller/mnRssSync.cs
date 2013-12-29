using My_News.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
namespace My_News
{
    public delegate void RssSyncProcessChange(ProgressChangedEventArgs e);
    public delegate void RssSyncDone();
    class mnRssSync
    {
        public static mnRssSync Instance;

        private RssReader rss = new RssReader();
        private List<Rsslink> list = new List<Rsslink>();
        private List<Item> listItem = new List<Item>();
        private BackgroundWorker bw = new BackgroundWorker();

        //
        public event RssSyncProcessChange RssSyncProcessChangeEvent;
        public event RssSyncDone RssSyncDoneEvent;

        private bool isRunning = false;
        private const int SYNC_TIME_DELAY = 1000;
        private int counter = 0;

        public mnRssSync()
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            RssSyncProcessChangeEvent(e);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Save to database
            if (listItem.Count > 0)
            {
                Logger.Instance.Log("mnRssSync.StartSync", String.Format("Sync Done: Rsslink {0}, Item: {1}", list.Count.ToString(), listItem.Count.ToString()));

                //Save settings
                Properties.Settings.Default.LastUpdate = DateTime.Now;
                Properties.Settings.Default.Save();

                ItemDAL id = new ItemDAL();
                id.DeleteAllItem();
                foreach (Item item in listItem)
                {
                    id.InsertItem(item);
                }
                if (RssSyncDoneEvent != null)
                {
                    RssSyncDoneEvent();
                }
            }
            else
            {
                Logger.Instance.Log("mnRssSync.StartSync", "Sync FAIL!");
            }

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                Logger.Instance.Log("mnRssSync.StartSync", "Start sync, RssLink: " + list.Count.ToString());
                listItem = new List<Item>();
                foreach (Rsslink r in list)
                {
                    try
                    {
                        rss.FeedUrl = r.Url;
                        foreach (RssItem item in rss.Execute())
                        {
                            if ((worker.CancellationPending == true))
                            {
                                e.Cancel = true;
                                break;
                            }
                            else
                            {
                                Item i;
                                DateTime item_date = DateTime.Now;

                                if (item.Date.ToBinary() != 0)
                                {
                                    item_date = item.Date;
                                }
                                i = new Item(0, r.Id, item.Title, item.Description, item.Link, item_date, "");
                                //Check for days
                                if ((DateTime.Now - item_date).TotalDays < Properties.Settings.Default.NewsDays)
                                {
                                    listItem.Add(i);
                                }
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        Logger.Instance.Log("mnRssSync.bw_DoWork", "Link error: " + r.Url + " >>: " + ee.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Log("mnRssSync.StartSync Exception:", ex.ToString());
            }
        }
        private void GetAllRssLink()
        {
            List<CaSo> listCaSo = new List<CaSo>();
            listCaSo = (new CaSoDAL()).GetUserSelectedCaSo();

            list = new List<Rsslink>();

            foreach (CaSo c in listCaSo)
            {
                List<Rsslink> l = new List<Rsslink>();
                l = (new RsslinkDAL()).GetRsslinksByCaSo(c);
                foreach (Rsslink rl in l)
                {
                    list.Add(rl);
                }
            }

            if (list != null)
            {
                Logger.Instance.Log("RssSync.GetAllRssLink", list.Count.ToString());
            }
            else
            {
                list = new List<Rsslink>();
            }
        }
        public void Run()
        {
            isRunning = true;
            counter = 0;
            StartSync();
            StartCounter();
        }

        public void StartSync()
        {
            GetAllRssLink();
            try
            {
                bw.CancelAsync();
                bw.RunWorkerAsync();
            }
            catch (Exception e)
            {
                Logger.Instance.Log("mmRssSync.StartSync", e.ToString());
            }
        }
        private void StartCounter()
        {
            while (isRunning)
            {
                ///Logger.Instance.Log("mnRssSync.StartCounter", String.Format("Tick! ({0}/{1})", counter, Properties.Settings.Default.SyncDelay * 60));
                System.Threading.Thread.Sleep(SYNC_TIME_DELAY);
                counter++;
                if (counter > Properties.Settings.Default.SyncDelay * 60) //SyncDelay * 60
                {
                    counter = 0;
                    StartSync();
                }
            }
        }
        public void Stop()
        {
            isRunning = false;
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
            }
        }
        public static mnRssSync GetInstance()
        {
            if (mnRssSync.Instance == null)
                mnRssSync.Instance = new mnRssSync();

            return mnRssSync.Instance;
        }
    }
}
