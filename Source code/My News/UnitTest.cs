using My_News.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace My_News
{
    static class UnitTest
    {
        public static void TestDbConnect()
        {
            Logger.Instance.Log("TestDBConnect", DbHelper.connection.ConnectionString);
            if (DbHelper.IsConnected())
            {
                Logger.Instance.Log("TestDBConnect", String.Format("Database connection OK!"));
            }
            else
            {
                Logger.Instance.Log("TestDBConnect", String.Format("Database connection fail!"));
            }

        }

        public static void TestInsertRsslink()
        {
            Rsslink rss = new Rsslink(1, "", new Category(2, "congnghe"), new Source(2, "dantri"));
            bool rs = (new RsslinkDAL()).InsertRsslink(rss);
            Logger.Instance.Log("TestCreateRsslink", rs.ToString());
        }

        public static void TestInsertCategory()
        {
            Category cat = new Category();
            cat.Id = 1;
            cat.Name = "haha";
            bool ok = (new CategoryDAL()).InsertCategory(cat);
            Logger.Instance.Log("TestCreateCategory", ok.ToString());
        }
        public static void TestUpdateCategory()
        {
            Category cat = new Category();
            cat.Id = 1;
            cat.Name = "HEHE";
            bool ok = (new CategoryDAL()).UpdateCategory(cat);
            Logger.Instance.Log("TestUpdateCategory", ok.ToString());
        }
        public static void TestDeleteCategory()
        {
            bool ok = (new CategoryDAL()).DeleteCategory(1);
            Logger.Instance.Log("TestDeleteCategory", ok.ToString());
        }
        public static void TestGetAllCategory()
        {
            List<Category> list = new List<Category>();
            list = (new CategoryDAL()).GetAllCategory();

            Logger.Instance.Log("TestGetAllCategory", list.Count.ToString());
        }
        public static void TestGetCategoryById()
        {
            Category cat = new Category();
            if ((cat = (new CategoryDAL()).GetCategoryById(1)) != null)
            {
                Logger.Instance.Log("TestGetCategoryById", cat.Name);
            }
            else
            {
                Logger.Instance.Log("TestGetCategoryById", "OK! (but return null)");
            }
        }

        public static void TestUpdateRsslink()
        {
            Rsslink rss = new Rsslink(1, "acb", new Category(2, "congnghe"), new Source(2, "dantri"));
            bool rs = (new RsslinkDAL()).UpdateRsslink(rss);
            Logger.Instance.Log("TestUpdateRsslink", rs.ToString());
        }

        public static void TestDeleteRsslink()
        {
            bool rs = (new RsslinkDAL()).DeleteRsslink(1);
            Logger.Instance.Log("TestDeleteRsslink", rs.ToString());
        }

        public static void TestGetAllRsslink()
        {
            List<Rsslink> list = new List<Rsslink>();
            list = (new RsslinkDAL()).GetAllRsslink();
            Logger.Instance.Log("TestGetAllRsslink", list.Count.ToString());
        }

        public static void TestGetRsslinkById()
        {
            Rsslink rss = new Rsslink();
            rss = (new RsslinkDAL()).GetRsslinkById(1);
            Logger.Instance.Log("TestGetRsslinkById", rss.Url.ToString());
        }

        public static void TestInsertSource()
        {
            Source sou = new Source();
            sou.Id = 1;
            sou.Name = "vnexpress";
            sou.Homepage = "abs";
            sou.Logo = "sba";
            bool rs = (new SourceDAL()).InsertSource(sou);
            Logger.Instance.Log("TestCreateSource", rs.ToString());
        }

        public static void TestUpdateSource()
        {
            Source sou = new Source();
            sou.Id = 1;
            sou.Name = "vnexpress";
            sou.Homepage = "aaaa";
            sou.Logo = "bbbb";
            bool rs = (new SourceDAL()).UpdateSource(sou);
            Logger.Instance.Log("TestUpdateSource", rs.ToString());
        }

        public static void TestDeleteSource()
        {
            bool rs = (new SourceDAL()).DeleteSource(1);
            Logger.Instance.Log("TestDeleteSource", rs.ToString());
        }

        public static void TestGetAllSource()
        {
            List<Source> list = new List<Source>();
            list = (new SourceDAL()).GetAllSource();
            Logger.Instance.Log("TestGetAllSource", list.Count.ToString());
        }

        public static void TestGetSourceById()
        {
            Source sou = new Source();
            sou = (new SourceDAL()).GetSourceById(1);
            Logger.Instance.Log("TestGetAllSourceById", sou.Name.ToString());
        }
        public static void TestInsertItem()
        {
            Item item = new Item();
            item.Id = 1;
            item.RsslinkId = 3;
            item.Link = "www.dantri.com";
            item.Thumbnail = "lo.jpg";
            item.Title = "Maria Ozawa";
            item.Date = DateTime.Now;
            item.Description = "Hello";
            bool rs = (new ItemDAL()).InsertItem(item);
            Logger.Instance.Log("TestInsertItem", rs.ToString());
        }

        public static void TestRssReader()
        {
            RssReader rss = new RssReader();
            rss.FeedUrl = "http://dantri.com.vn/the-thao.rss";
            foreach (RssItem item in rss.Execute())
            {
                Logger.Instance.Log("TestRssReader", item.Title);
            }
        }

        public static void TestmnRssSync()
        {
            mnRssSync.Instance.RssSyncDoneEvent += TestRssSyncDoneEvent;
            mnRssSync.Instance.Run();
        }
        public static void TestRssSyncDoneEvent()
        {
            Logger.Instance.Log("TestmnRssSync", "SYNC DONE!");
        }
        public static void TestUpdateItem()
        {
            Item item = new Item();
            item.Id = 1;
            item.Link = "www.dantrixsssxx.com";
            item.Thumbnail = "sss.jpg";
            item.Title = "Mariasss Ozawa";
            item.Date = DateTime.Now;
            item.Description = "Helsslo";
            bool rs = (new ItemDAL()).UpdateItem(item);
            Logger.Instance.Log("TestUpdateItem", rs.ToString());
        }
        public static void TestDeleteItem()
        {
            bool rs = (new ItemDAL()).DeleteItem(1);

            Logger.Instance.Log("TestDeleteItem", rs.ToString());
        }

        public static void TestGetAllItem()
        {
            List<Item> list = new List<Item>();
            list = (new ItemDAL()).GetAllItem();
            Logger.Instance.Log("TestGetAllItem", list.Count.ToString());
        }
        public static void TestGetItemById()
        {
            Item item = new Item();
            item = (new ItemDAL()).GetItemById(1);
            Logger.Instance.Log("TestGetAllItem", item.Title.ToString());
        }
        public static void TestExistsCategory()
        {
            bool rs = (new CategoryDAL()).ExistsCategory("The thao");
            Logger.Instance.Log("TestExistsCategory", rs.ToString());
        }

        public static void TestGetSourceByName()
        {
            Source sou = (new SourceDAL()).GetSourceByName("asd");
            Logger.Instance.Log("TestGetSourceByNmae", sou.Name);
        }

        public static void TestGetCategoryByName()
        {
            Category cat = (new CategoryDAL()).GetCategoryByName("dsa");
            Logger.Instance.Log("TestGetCategoryByNmae", cat.Name);
        }
        public static void TestGetItemsByRsslink()
        {
            List<Item> list = new List<Item>();
            list = (new ItemDAL()).GetItemsByRsslink(1);
            Logger.Instance.Log("TestGetItemsByRsslink", list.Count.ToString());
        }
    }
}
