using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    // Entity Framework context
   public class PlayerDbContext : DbContext
    {      
        
        public PlayerDbContext() : base("stabberPlayersContext")
        {
            // when developing this is used to renew the database
            Database.SetInitializer<PlayerDbContext>(new DropCreateDatabaseAlways<PlayerDbContext>());
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Gold> Golds { get; set; }        
        public DbSet<HealthPotion> HealthPotions { get; set; }        

    }
}
