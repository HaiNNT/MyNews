using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace My_News
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                //instance of the process already active, exit
                return;
            }


            Logger logger = Logger.GetInstance("MY_NEWS", true, true);
            mnRssSync rssSync = mnRssSync.GetInstance();

            #region TestCases
            //UnitTest.TestDbConnect();

            //UnitTest.TestInsertCategory();
            //UnitTest.TestInsertCategory();
            //UnitTest.TestUpdateCategory();
            //UnitTest.TestGetAllCategory();
            //UnitTest.TestGetCategoryById();
            //UnitTest.TestDeleteCategory();
            //UnitTest.TestDeleteCategory();
            //UnitTest.TestExistsCategory();



            //UnitTest.TestInsertSource();
            //UnitTest.TestUpdateSource();
            //UnitTest.TestGetAllSource();
            //UnitTest.TestGetSourceById();
            //UnitTest.TestDeleteSource();
            //UnitTest.TestDeleteSource();

            //UnitTest.TestInsertCategory();
            //UnitTest.TestInsertSource();
            //UnitTest.TestInsertRsslink();
            //UnitTest.TestGetAllRsslink();
            //UnitTest.TestGetRsslinkById();
            //UnitTest.TestUpdateRsslink();
            //UnitTest.TestDeleteRsslink();
            //UnitTest.TestDeleteRsslink();

            //UnitTest.TestDeleteSource();
            //UnitTest.TestDeleteCategory();

            //UnitTest.TestInsertItem();
            //UnitTest.TestUpdateItem();
            //UnitTest.TestGetAllItem();
            //UnitTest.TestGetItemById();
            //UnitTest.TestDeleteItem();
            //UnitTest.TestDeleteItem();

            //UnitTest.TestExistsCategory();
            //UnitTest.TestGetSourceByName();
            //UnitTest.TestGetCategoryByName();

            //UnitTest.TestGetItemsByRsslink();

            //UnitTest.TestRssReader();

            //UnitTest.TestmnRssSync();
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*
             * First run
             *  = 0: Lần đầu tiên chạy.
             *  = 1: Kết nối DB thành công, nhưng không tạo được DB vì quyền admin
             *  = 2: tạo, import database thành công.
             */

            if (Properties.Settings.Default.FirstRun < 2)
            {
                Application.Run(new frmFirstRun());
            }
            else
            {
                DbHelper.NoServerMode = !DbHelper.IsConnected();

                if (!DbHelper.NoServerMode)
                {
                    Thread autoSyncThread = new Thread(new ThreadStart(mnRssSync.Instance.Run));
                    autoSyncThread.Start();
                }

                frmMyNews frmMyNews_instance = frmMyNews.GetInstance();
                frmMain frmMain_instance = frmMain.GetInstance();

                Application.Run(frmMyNews_instance);
            }

        }
    }
}
