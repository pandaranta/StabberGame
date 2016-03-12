using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace StabberServer
{
    class UserDBContext : DbContext
    {
        public UserDBContext() :base ("serverPlayersContext")
        {
            // when developing this is used to renew the database
           // Database.SetInitializer<PlayerDbContext>(new DropCreateDatabaseAlways<PlayerDbContext>());
        }
        public DbSet<User> Users { get; set; }


    }
}
