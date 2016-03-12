using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Stabber
{
    // Entity framework context
   public class WorldDbContext : DbContext
    {
        public WorldDbContext() : base("stabberWorldContext")
        {
            // when developing this is used to renew the database
            Database.SetInitializer<WorldDbContext>(new DropCreateDatabaseAlways<WorldDbContext>());
        }

        public DbSet<World> Worlds {get;set;}

    }
}
