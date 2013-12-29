using System;
using System.Collections.Generic;
using System.Text;

namespace My_News
{
    class CaSo: IComparable
    {
        public Category Category { get; set; }
        public Source Source { get; set; } 
        public CaSo(Category cat = null, Source sou = null)
        {
            Category = cat;
            Source = sou;
        }

        public int CompareTo(Object caso)
        {
            if (caso == null) return 1;

            CaSo othercaso = caso as CaSo;
            if (othercaso != null)
                return this.Category.Name.CompareTo(othercaso.Category.Name);
            else
                throw new ArgumentException("Object is not a CaSo");
        }

        

   
}

    
}
