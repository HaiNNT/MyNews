using System;
using System.Collections.Generic;
using System.Text;

namespace My_News
{
    class Rsslink
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Category GetCategory { get; set; }
        public Source GetSource { get; set; }

        public Rsslink()
        {
            Id = 0;
            Url = "";
            GetCategory = new Category();
            GetSource = new Source();
        }
        public Rsslink(int id , string url , Category cat , Source sou)
        {
            Id = id;
            Url = url;
            GetCategory = cat;
            GetSource = sou;
 
        }
    }


}
