using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
   public class World
    {
        public int WorldId { get; set; }
        public string JsonWorld { get; set; }

        public World(string jsonWorld)
        {
            JsonWorld = jsonWorld;
        }
    }
}
