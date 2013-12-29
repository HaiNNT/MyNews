using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace My_News.DAL
{
    class SourceDAL
    {
        public bool InsertSource(Source sou)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Source VALUES( @sou_name, @sou_home, @sou_logo)", DbHelper.connection);
                cmd.Parameters.AddWithValue("sou_id", sou.Id);
                cmd.Parameters.AddWithValue("sou_name", sou.Name);
                cmd.Parameters.AddWithValue("sou_home", sou.Homepage);
                cmd.Parameters.AddWithValue("sou_logo", sou.Logo);
                DbHelper.Open();
                int result = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return result > 0;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL CreateSource", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public bool UpdateSource(Source sou)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Source SET SourceName = @sou_name, SourceHomepage = @sou_home, SourceLogo = @sou_logo WHERE SourceId = @sou_id", DbHelper.connection);
                cmd.Parameters.AddWithValue("sou_id", sou.Id);
                cmd.Parameters.AddWithValue("sou_name", sou.Name);
                cmd.Parameters.AddWithValue("sou_home", sou.Homepage);
                cmd.Parameters.AddWithValue("sou_logo", sou.Logo);
                DbHelper.Open();
                int result = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return result > 0;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL UpdateSource", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public bool DeleteSource(int sou_id)
        {
            try
            {
                bool rs = (new RsslinkDAL()).DeleteRsslinkBySourceId(sou_id);
                if (rs)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Source WHERE SourceId = @sou_id", DbHelper.connection);
                    cmd.Parameters.AddWithValue("sou_id", sou_id);
                    DbHelper.Open();
                    int result = cmd.ExecuteNonQuery();
                    DbHelper.Close();
                    return result > 0;
                }
                else
                {
                    Logger.Instance.Log("RsslinkDAL DeleteRsslinkBySourceId", rs.ToString());
                    DbHelper.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL UpdateSource", e.ToString());
                return false;
            }
        }
        public List<Source> GetAllSource()
        {
            List<Source> list = new List<Source>();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Source", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Source sou = new Source();

                        sou.Id = Convert.ToInt32(sdr[0]);
                        sou.Name = Convert.ToString(sdr[1]);
                        sou.Homepage = Convert.ToString(sdr[2]);
                        sou.Logo = Convert.ToString(sdr[3]);

                        list.Add(sou);
                    }
                }

                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL GetAllSource", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Source GetSourceById(int sou_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Source WHERE SourceId = @sou_id", DbHelper.connection);
                cmd.Parameters.AddWithValue("sou_id", sou_id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                sdr.Read();

                Source sou = new Source();

                sou.Id = Convert.ToInt32(sdr[0]);
                sou.Name = Convert.ToString(sdr[1]);
                sou.Homepage = Convert.ToString(sdr[2]);
                sou.Logo = Convert.ToString(sdr[3]);
                DbHelper.Close();
                return sou;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL GetSourceById", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Source GetSourceByName(string name)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Source WHERE SourceName = @name", DbHelper.connection);
                cmd.Parameters.AddWithValue("name", name);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                sdr.Read();

                Source sou = new Source();

                sou.Id = Convert.ToInt32(sdr[0]);
                sou.Name = Convert.ToString(sdr[1]);
                sou.Homepage = Convert.ToString(sdr[2]);
                sou.Logo = Convert.ToString(sdr[3]);
                DbHelper.Close();
                return sou;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("SourceDAL GetSourceByName", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        
    }
}
