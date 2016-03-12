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
        public int GoldId { get; set; }

        // Constructor
        public Gold() : base("Golden Nugget")
        {

        }    

    }

    // Items used by players in game.
    class HealthPotion : Item
    {
        public int HealthPotionId { get; set; }

        //Constructor
        public HealthPotion() : base("Health Potion")
        {

        }

    }
}
