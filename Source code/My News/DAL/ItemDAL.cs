using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace My_News.DAL
{
    class ItemDAL
    {
        public bool InsertItem(Item item)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Insert into Item values(@ItemRsslinkId,@ItemTitle,@ItemDescription,@ItemLink,@ItemDate,@ItemThumbnail)", DbHelper.connection);

                cmd.Parameters.AddWithValue("ItemRsslinkId", item.RsslinkId);
                cmd.Parameters.AddWithValue("ItemTitle", item.Title);
                cmd.Parameters.AddWithValue("ItemDescription", item.Description);
                cmd.Parameters.AddWithValue("ItemLink", item.Link);
                cmd.Parameters.AddWithValue("ItemDate", item.Date);
                cmd.Parameters.AddWithValue("ItemThumbnail", item.Thumbnail);

                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL.Insert ", e.ToString());
                DbHelper.Close();
                return false;
            }
        }

        public bool DeleteItem(int item_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete from Item where ItemId = @Item_Id", DbHelper.connection);
                cmd.Parameters.AddWithValue("Item_Id", item_id);
                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL.Delete", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public bool UpdateItem(Item item)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Update Item Set ItemTitle=@ItemTitle,ItemDescription=@ItemDescription,ItemLink=@ItemLink,ItemDate=@ItemDate,ItemThumbnail=@ItemThumbnail Where ItemId=@ItemId", DbHelper.connection);
                cmd.Parameters.AddWithValue("ItemId", item.Id);
                cmd.Parameters.AddWithValue("ItemTitle", item.Title);
                cmd.Parameters.AddWithValue("ItemDescription", item.Description);
                cmd.Parameters.AddWithValue("ItemLink", item.Link);
                cmd.Parameters.AddWithValue("ItemDate", item.Date);
                cmd.Parameters.AddWithValue("ItemThumbnail", item.Thumbnail);

                DbHelper.Open();
                int count = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return (count == 1);
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL.UpdateItem", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
        public List<Item> GetAllItem()
        {
            List<Item> list = new List<Item>();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Item ORDER BY NEWID()", DbHelper.connection);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Item item = new Item();

                        item.Id = Convert.ToInt32(sdr[0]);
                        item.RsslinkId = Convert.ToInt32(sdr[1]);
                        item.Title = Convert.ToString(sdr[2]);
                        item.Description = Convert.ToString(sdr[3]);
                        item.Link = Convert.ToString(sdr[4]);
                        item.Date = Convert.ToDateTime(sdr[5]);
                        item.Thumbnail = Convert.ToString(sdr[6]);



                        list.Add(item);
                    }
                }

                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL GetAllItem", e.ToString());
                DbHelper.Close();
                return null;
            }
        }
        public List<Item> GetItemsByRsslink(int rsslink_id)
        {
            List<Item> list = new List<Item>();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Item WHERE RsslinkId = @rsslink_id ORDER BY NEWID()", DbHelper.connection);
                cmd.Parameters.AddWithValue("rsslink_id", rsslink_id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Item item = new Item();

                        item.Id = Convert.ToInt32(sdr[0]);
                        item.RsslinkId = Convert.ToInt32(sdr[1]);
                        item.Title = Convert.ToString(sdr[2]);
                        item.Description = Convert.ToString(sdr[3]);
                        item.Link = Convert.ToString(sdr[4]);
                        item.Date = Convert.ToDateTime(sdr[5]);
                        item.Thumbnail = Convert.ToString(sdr[6]);



                        list.Add(item);
                    }
                }

                DbHelper.Close();
                return list;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL GetAllItem", e.ToString());
                DbHelper.Close();
                return null;
            }
        }

        public Item GetItemById(int item_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Item WHERE ItemId = @item_id", DbHelper.connection);
                cmd.Parameters.AddWithValue("item_id", item_id);
                DbHelper.Open();
                SqlDataReader sdr = cmd.ExecuteReader();

                sdr.Read();

                Item item = new Item();

                item.Id = Convert.ToInt32(sdr[0]);
                item.RsslinkId = Convert.ToInt32(sdr[1]);
                item.Title = Convert.ToString(sdr[2]);
                item.Description = Convert.ToString(sdr[3]);
                item.Link = Convert.ToString(sdr[4]);
                item.Date = Convert.ToDateTime(sdr[5]);
                item.Thumbnail = Convert.ToString(sdr[6]);

                DbHelper.Close();
                return item;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL GetItemById", e.ToString());
                DbHelper.Close();
                return null;
            }
        }
        public bool DeleteAllItem()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("TRUNCATE TABLE Item", DbHelper.connection);
                DbHelper.Open();
                int result = cmd.ExecuteNonQuery();
                DbHelper.Close();
                return true;
            }
            catch (Exception e)
            {
                Logger.Instance.Log("ItemDAL DeleteAllItem", e.ToString());
                DbHelper.Close();
                return false;
            }
        }
    }
}
