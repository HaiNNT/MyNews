using System;
using System.Collections.Generic;
using System.Text;

namespace My_News
{
    public class Item
    {
        public int Id { get; set; }
        public int RsslinkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public string Thumbnail { get; set; }
        
        public Item(int id = 0, int rsslinkId = 0, string title = "", string description ="", string link="", DateTime date = (new DateTime()), string thumbnail="")
        {
            Id = id;
            RsslinkId = rsslinkId;
            Title = title;
            Description = description;
            Link = link;
            Date = date;
            Thumbnail = thumbnail;
        }
            
    }
}
