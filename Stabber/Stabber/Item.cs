using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    // Items used by players in game.
    abstract class Item
    {
        public string Name { get; set; }

        //Constructor
        public Item(string name)
        {
            Name = name;
        }

    }

    // Items used by players in game.
    class Gold : Item
    {
        // Constructor
        public Gold() : base("Golden nugget")
        {

        }      

    }
}
