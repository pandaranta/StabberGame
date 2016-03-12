using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    // Entity Framework context
   public class StatisticDbContext : DbContext
    {
        public StatisticDbContext() : base("stabberStatsContext")
        {
            // when developing this is used to renew the database
            Database.SetInitializer<StatisticDbContext>(new DropCreateDatabaseAlways<StatisticDbContext>());

        }

        public DbSet<Statistic> Statistics { get; set; }
    }
}
