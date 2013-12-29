using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace My_News.DAL
{
    class CaSoDAL
    {
        public List<CaSo> GetAllCaSo()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select distinct * From (Select s.*, c.* from Rsslink r, Source s, Category c WHERE r.SourceId = s.SourceId AND r.CategoryId = c.CategoryId)a", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<CaSo> list = new List<CaSo>();
                while (sdr.Read())
                {
                    Category cat = new Category();
                    Source sou = new Source();
                    sou.Id = (int)sdr[0];
                    sou.Name = (string)sdr[1];
                    sou.Homepage = (string)sdr[2];
                    sou.Logo = (string)sdr[3];
                    cat.Id = (int)sdr[4];
                    cat.Name = (string)sdr[5];
                    CaSo caso = new CaSo(cat, sou);
                    list.Add(caso);
                }
                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CaSoDAL.GetAllCaSo", e.ToString());
                DbHelper.Close();
                return null;
            }
        }


        /// <summary>
        /// Lưu lại toàn bộ cài đặt của người dùng.
        /// TODO: CHange when update to multi-user.
        /// </summary>
        /// <param name="list">Danh sách cách Category, Source mà người dùng đã chọn</param>
        /// <returns></returns>
        public bool SetUserSelectedCaSo(List<CaSo> list, int user_id = 0) {
            
            //Delete all previous settings
            SqlCommand cmd = new SqlCommand("TRUNCATE TABLE User_Select", DbHelper.connection);
            DbHelper.Open();
            cmd.ExecuteNonQuery();
            DbHelper.Close();

            //Insert new settings
            try
            {
                int count = 0;
                foreach (var item in list)
                {
                    cmd = new SqlCommand("Insert into User_Select values (@UserId, @CategoryId, @SourceId)", DbHelper.connection);
                    DbHelper.Open();
                    cmd.Parameters.AddWithValue("CategoryId", item.Category.Id);
                    cmd.Parameters.AddWithValue("SourceId", item.Source.Id);
                    cmd.Parameters.AddWithValue("UserId", user_id);
                    cmd.ExecuteNonQuery();
                    count++;
                    DbHelper.Close();
                }
                return (count == list.Count);

            }
            catch (Exception e)
            {
                Logger.Instance.Log("CaSoDAL.SetUserSelectedCaSo", e.ToString());
                DbHelper.Close();
                return false;
            }
            
           
        }

        public List<CaSo> GetUserSelectedCaSo(int user_id = 0)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select s.*, c.* from User_Select u, Source s, Category c WHERE u.SourceId = s.SourceId AND u.CategoryId = c.CategoryId ", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<CaSo> list = new List<CaSo>();
                while (sdr.Read())
                {
                    Category cat = new Category();
                    Source sou = new Source();
                    sou.Id = (int)sdr[0];
                    sou.Name = (string)sdr[1];
                    sou.Homepage = (string)sdr[2];
                    sou.Logo = (string)sdr[3];
                    cat.Id = (int)sdr[4];
                    cat.Name = (string)sdr[5];
                    CaSo caso = new CaSo(cat, sou);
                    list.Add(caso);
                }
                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CaSoDAL.GetUserSelectedCaSo", e.ToString());
                DbHelper.Close();
                return null;
            }
        }
    }
}
