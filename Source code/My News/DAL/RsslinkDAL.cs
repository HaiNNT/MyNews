using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace My_News.DAL
{
    class RsslinkDAL
    {
        public bool InsertRsslink(Rsslink rss)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Insert into Rsslink values ( @RsslinkUrl, @SourceID, @CategoryID)", DbHelper.connection);
                cmd.Parameters.AddWithValue("RsslinkID", rss.Id);
                cmd.Parameters.AddWithValue("RsslinkUrl", rss.Url);
                cmd.Parameters.AddWithValue("SourceID", rss.GetSource.Id);
                cmd.Parameters.AddWithValue("CategoryID", rss.GetCategory.Id);
                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {

                Logger.Instance.Log("RsslinkDAL.InsertRsslink", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

        public bool UpdateRsslink(Rsslink rss)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Update Rsslink set RsslinkUrl = @RsslinkUrl, SourceID = @SourceId, CategoryID = @CategoryId where RsslinkID = @RsslinkId", DbHelper.connection);
                cmd.Parameters.AddWithValue("RsslinkID", rss.Id);
                cmd.Parameters.AddWithValue("RsslinkUrl", rss.Url);
                cmd.Parameters.AddWithValue("SourceID", rss.GetSource.Id);
                cmd.Parameters.AddWithValue("CategoryID", rss.GetCategory.Id);
                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.UpdateRsslink", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

        public bool DeleteRsslink(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete from Rsslink where RsslinkId = @id", DbHelper.connection);
                cmd.Parameters.AddWithValue("id", id);
                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.DeleteRsslink", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

        public List<Rsslink> GetAllRsslink()
        {
            DbHelper.Close();
            try
            {
                SqlCommand cmd = new SqlCommand("Select r.RsslinkId, r.RsslinkUrl, s.SourceId, s.SourceName, s.SourceHomepage, s.SourceLogo, c.CategoryId, c.CategoryName from Rsslink r, Source s, Category c WHERE r.SourceId = s.SourceId AND r.CategoryId = c.CategoryId ", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Rsslink> list = new List<Rsslink>();
                while (sdr.Read())
                {
                    Rsslink rss = new Rsslink();
                    rss.Id = (int)sdr[0];
                    rss.Url = (string)sdr[1];
                    rss.GetSource.Id = (int)sdr[2];
                    rss.GetSource.Name = (string)sdr[3];
                    rss.GetSource.Homepage = (string)sdr[4];
                    rss.GetSource.Logo = (string)sdr[5];
                    rss.GetCategory.Id = (int)sdr[6];
                    rss.GetCategory.Name = (string)sdr[7];
                    list.Add(rss);
                }
                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.GetAllRsslink", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Rsslink GetRsslinkById(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select Rsslink.RsslinkId, Rsslink.RsslinkUrl, Source.*, Category.* from Rsslink, Source, Category Where RsslinkID = @id  AND Rsslink.CategoryId = Category.CategoryId AND Rsslink.SourceId = Source.SourceId", DbHelper.connection);
                cmd.Parameters.AddWithValue("id", id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();
                Rsslink rss = new Rsslink();
                rss.Id = id;
                rss.Url = Convert.ToString(sdr[1]);
                rss.GetSource.Id = (int)sdr[2];
                rss.GetSource.Name = (string)sdr[3];
                rss.GetSource.Homepage = (string)sdr[4];
                rss.GetSource.Logo = (string)sdr[5];
                rss.GetCategory.Id = (int)sdr[6];
                rss.GetCategory.Name = (string)sdr[7];
                DbHelper.Close();
                return rss;
            }
            catch (Exception e)
            {

                Logger.Instance.Log("RsslinkDAL.GetRsslinkById", e.ToString());
                DbHelper.Close();
                return null;
            }

        }

        public List<Rsslink> GetRsslinksByCaSo(CaSo caso)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select Rsslink.RsslinkId, Rsslink.RsslinkUrl, Source.*, Category.* from Rsslink, Source, Category WHERE Rsslink.CategoryId = @cat_id AND Rsslink.SourceId = @sou_id  AND Rsslink.CategoryId = Category.CategoryId AND Rsslink.SourceId = Source.SourceId", DbHelper.connection);
                cmd.Parameters.AddWithValue("cat_id", caso.Category.Id);
                cmd.Parameters.AddWithValue("sou_id", caso.Source.Id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Rsslink> list = new List<Rsslink>();
                while (sdr.Read())
                {
                    Rsslink rss = new Rsslink();
                    rss.Id = (int)sdr[0];
                    rss.Url = (string)sdr[1];
                    rss.GetSource.Id = (int)sdr[2];
                    rss.GetSource.Name = (string)sdr[3];
                    rss.GetSource.Homepage = (string)sdr[4];
                    rss.GetSource.Logo = (string)sdr[5];
                    rss.GetCategory.Id = (int)sdr[6];
                    rss.GetCategory.Name = (string)sdr[7];
                    list.Add(rss);
                }
                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.GetRsslinksByCaSo", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public bool DeleteRsslinkBySourceId(int id)
        {
            try
            {
                SqlCommand cmd1 = new SqlCommand("Select * from Rsslink where SourceId = @id", DbHelper.connection);
                cmd1.Parameters.AddWithValue("id", id);
                DbHelper.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();
                bool ok = sdr.HasRows;
                DbHelper.Close();
                if (ok)
                {
                    SqlCommand cmd = new SqlCommand("Delete from Rsslink where SourceId = @id", DbHelper.connection);
                    cmd.Parameters.AddWithValue("id", id);
                    DbHelper.Open();
                    int count = cmd.ExecuteNonQuery();
                    DbHelper.Close();
                    return (count > 0);
                }
                else
                {
                    DbHelper.Close();
                    return true;
                }

            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.DeleteRsslinkBySourceID", e.ToString());
                DbHelper.Close();
                return false;
            }
        }


        public bool DeleteRsslinkByCategoryId(int id)
        {
            try
            {
                SqlCommand cmd1 = new SqlCommand("Select * from Rsslink where CategoryId = @id", DbHelper.connection);
                cmd1.Parameters.AddWithValue("id", id);
                DbHelper.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();
                bool ok = sdr.HasRows;
                DbHelper.Close();
                if (ok)
                {
                    SqlCommand cmd2 = new SqlCommand("Delete from Rsslink where CategoryId = @id", DbHelper.connection);
                    cmd2.Parameters.AddWithValue("id", id);
                    DbHelper.Open();
                    int count = cmd2.ExecuteNonQuery();
                    DbHelper.Close();
                    return (count > 0);
                }
                else
                {
                    DbHelper.Close();
                    return true;
                }

            }
            catch (Exception e)
            {
                Logger.Instance.Log("RsslinkDAL.DeleteRsslinkByCategoryID", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

    }
}
