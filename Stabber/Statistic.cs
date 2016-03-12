using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Stabber
{
   public class Statistic
    {
        public int StatisticId { get; set; }        
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int GamesPlayed { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int MovesMade { get; set; }
        public int TimesMovedLeft { get; set; }
        public int TimesMovedRight { get; set; }
        public int TimesMovedUp { get; set; }
        public int TimesMovedDown { get; set; }
        public int TimesStabbed { get; set; }
        public int GoldAquired { get; set; }
        public int TotalHealthLost { get; set; }
        public int TotalHealthRegenerated { get; set; }
        public int HealthPotionsPickedUp { get; set; }
        public int DamageDone { get; set; }
        public int DamageTaken { get; set; }

    }
}
