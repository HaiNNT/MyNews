using System;
using System.Collections.Generic;
using System.Text;

namespace My_News
{
    class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id = 0, string name = "")
        {
            Id = id;
            Name = name;
        }
    }


}
