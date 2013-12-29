using System;
using System.Collections.Generic;
using System.Text;

namespace My_News
{
    class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Homepage { get; set; }
        public string Logo { get; set; }

        public Source(int id=0, string name="", string hp ="", string logo="")
        {
            Id = id;
            Name = name;
            Homepage = hp;
            Logo = logo;
        }
    }
}
