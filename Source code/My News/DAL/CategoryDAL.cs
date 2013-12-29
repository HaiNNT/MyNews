using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace My_News.DAL
{
    class CategoryDAL
    {
        public bool InsertCategory(Category cat)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Category VALUES(@cat_name)", DbHelper.connection);
                cmd.Parameters.AddWithValue("cat_id", cat.Id);
                cmd.Parameters.AddWithValue("cat_name", cat.Name);
                DbHelper.Open();
                int result = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (result == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL InsertCategory", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public bool UpdateCategory(Category cat)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Category SET CategoryName = @cat_name WHERE CategoryId = @cat_id", DbHelper.connection);
                cmd.Parameters.AddWithValue("cat_id", cat.Id);
                cmd.Parameters.AddWithValue("cat_name", cat.Name);
                DbHelper.Open();
                int result = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return result > 0;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL UpdateCategory", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public bool DeleteCategory(int cat_id)
        {
            try
            {
                bool rs = (new RsslinkDAL()).DeleteRsslinkByCategoryId(cat_id);
                if (rs)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE CategoryId = @cat_id", DbHelper.connection);
                    cmd.Parameters.AddWithValue("cat_id", cat_id);
                    DbHelper.Open();
                    int result = cmd.ExecuteNonQuery();
                    DbHelper.Close();
                    return result > 0;
                }
                else
                {
                    Logger.Instance.Log("RsslinkDAL DeleteRsslinkByCategoryId", rs.ToString());
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL UpdateCategory", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public List<Category> GetAllCategory()
        {
            List<Category> list = new List<Category>();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Category cat = new Category();

                        cat.Id = Convert.ToInt32(sdr[0]);
                        cat.Name = Convert.ToString(sdr[1]);

                        list.Add(cat);
                    }
                }

                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL GetAllCategory", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Category GetCategoryById(int cat_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE CategoryId = @cat_id", DbHelper.connection);
                cmd.Parameters.AddWithValue("cat_id", cat_id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                sdr.Read();

                Category cat = new Category();

                cat.Id = Convert.ToInt32(sdr[0]);
                cat.Name = Convert.ToString(sdr[1]);
                DbHelper.Close();
                return cat;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL GetCategoryById", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Category GetCategoryByName(string name)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE CategoryName = @name", DbHelper.connection);
                cmd.Parameters.AddWithValue("name", name);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                sdr.Read();

                Category cat = new Category();

                cat.Id = Convert.ToInt32(sdr[0]);
                cat.Name = Convert.ToString(sdr[1]);
                DbHelper.Close();
                return cat;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("CategoryDAL GetCategoryByName", e.ToString());
                DbHelper.Close();
                return null;
            }
        }




        public bool ExistsCategory(string cat_name)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT CategoryId FROM Category WHERE CategoryName = @cat_name", DbHelper.connection);
                cmd.Parameters.AddWithValue("cat_name", cat_name);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                DbHelper.Close();

                return sdr.HasRows;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ExistsCategory", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

    }
}
