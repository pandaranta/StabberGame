using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    // The individual rooms making up the game world.
    class Room
    {        
        public List<Item> Contents { get; set; }
        public static Random random = new Random();        

        // Constructor.
        public Room()
        {            
            Contents = new List<Item>();
            if (random.Next(1, 101) > 90)
            {
                Contents.Add(new Gold());
            }

        }
        
        // Check if the room contains an item
        public bool HasItem()
        {            
            if (Contents.Count > 0)
            {
                return true;
            }
            else return false;
        }       

    }
}
