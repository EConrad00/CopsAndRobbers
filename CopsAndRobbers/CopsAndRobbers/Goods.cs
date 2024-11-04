using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopsAndRobbers
{
    internal class Goods
    {
        public string ItemName { get; set; }
        //public string OriginalOwner { get; }
        public Goods(string itemName)
        {
            ItemName = itemName;
        }
    }
    class Belongings : Goods
    {
        public Belongings(string stolenItemName) : base(stolenItemName)
        {

        }
    }

    class StolenGoods : Goods
    {
        public StolenGoods(string stolenItemName): base(stolenItemName) 
        {
            
        }
    }

    class SizedGoods : Goods
    {
        public SizedGoods(string sizedItemName) : base(sizedItemName)
        {

        }
    }
}
