using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    // This class contains player information and some player related logic.
    class Player
    {

        public static Random random = new Random();

        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int MaxHealth { get; set; }
        public List<Gold> Backpack { get; set; }


        // Constructor.
        public Player(int posX, int posY, int id, string name = "Nobody gave me a name...")
        {
            Backpack = new List<Gold>();

            MaxHealth = 5;
            Name = name;
            PosX = random.Next(posX);
            PosY = random.Next(posY);
            Damage = 1;
            Health = MaxHealth;
            PlayerId = id;
        }

        // Attacks another player.
        void Attack(Player opponent)
        {
            var dbs = new StatisticDbContext();            
            var opponentStats = dbs.Statistics.Find(opponent.PlayerId);            

            opponent.Health -= 1;
            opponentStats.TotalHealthLost += 1;
            opponentStats.DamageTaken += 1;
            opponentStats.TimesStabbed += 1;

            dbs.SaveChanges();
            
        }

        // Loots the item in the room.
        void LootRoom(Room room)
        {
            var dbs = new StatisticDbContext();

            var playerStats = dbs.Statistics.Find(this.PlayerId);

            foreach (var item in room.Contents)
            {
                if (item.Name == "Health Potion")
                {
                    this.Health += 2;
                    playerStats.HealthPotionsPickedUp += 1;
                    playerStats.TotalHealthRegenerated += 2;
                    
                }
                else if (item.Name == "Golden Nugget")
                {
                    Gold gold = new Gold();
                    this.Backpack.Add(gold);

                    playerStats.GoldAquired += 1;
                   
                }               

            }

            dbs.SaveChanges();
            room.Contents.Clear();
        }

        // Move the player and call Attack() and LootRoom().
        public bool Move(Game game, Player player, ConsoleKeyInfo key, Player opponent)
        {            

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (player.PosY > 0)
                    {
                        if (opponent.PosX == player.PosX && opponent.PosY == player.PosY - 1)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedLeft += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosY -= 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedLeft += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;

                        }

                        

                    }
                    else return false;
                    break;
                case ConsoleKey.A:
                    if (player.PosY > 0)
                    {
                        if (opponent.PosX == player.PosX && opponent.PosY == player.PosY - 1)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedLeft += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosY -= 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedLeft += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;

                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.DownArrow:
                    if (player.PosX < game.World.GetLength(0) - 1)
                    {
                        if (opponent.PosX == player.PosX + 1 && opponent.PosY == player.PosY)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedDown += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosX += 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedDown += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;

                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.S:
                    if (player.PosX < game.World.GetLength(0) - 1)
                    {
                        if (opponent.PosX == player.PosX + 1 && opponent.PosY == player.PosY)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedDown += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosX += 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedDown += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;

                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.RightArrow:
                    if (player.PosY < game.World.GetLength(1) - 1)
                    {
                        if (opponent.PosX == player.PosX && opponent.PosY == player.PosY + 1)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedRight += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosY += 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedRight += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.D:
                    if (player.PosY < game.World.GetLength(1) - 1)
                    {
                        if (opponent.PosX == player.PosX && opponent.PosY == player.PosY + 1)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedRight += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosY += 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedRight += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.UpArrow:
                    if (player.PosX > 0)
                    {
                        if (opponent.PosX == player.PosX - 1 && opponent.PosY == player.PosY)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedUp += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosX -= 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedUp += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                    }
                    else return false;
                    break;
                case ConsoleKey.W:
                    if (player.PosX > 0)
                    {
                        if (opponent.PosX == player.PosX - 1 && opponent.PosY == player.PosY)
                        {
                            player.Attack(opponent);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.DamageDone += 1;
                            playerStats.TimesMovedUp += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                        else
                        {
                            player.PosX -= 1;
                            player.LootRoom(game.World[player.PosX, player.PosY]);

                            var dbs = new StatisticDbContext();
                            var playerStats = dbs.Statistics.Find(player.PlayerId);
                            playerStats.TimesMovedUp += 1;
                            playerStats.MovesMade += 1;

                            dbs.SaveChanges();

                            return true;
                        }
                    }
                    else return false;
                    break;

                default: return false;
            }
        }

        // Check if a player is dead. If so, drop inventory, reset health and respawn at new location.
        public bool CheckIfDeadRespawn(Game game, Player player, Player opponent)
        {
            if (player.Health <= 0)
            {
                foreach (var item in player.Backpack)
                {
                    game.World[player.PosX, player.PosY].Contents.Add(item);
                }
                player.Backpack.Clear();

                player.PosX = random.Next(game.World.GetLength(0));
                player.PosY = random.Next(game.World.GetLength(1));
                player.Health = player.MaxHealth;

                var dbs = new StatisticDbContext();
                var playerStats = dbs.Statistics.Find(player.PlayerId);
                var opponentStats = dbs.Statistics.Find(opponent.PlayerId);

                playerStats.Deaths += 1;
                opponentStats.Kills += 1;
                dbs.SaveChanges();

                return true;
            }
            if (opponent.Health <= 0)
            {
                foreach (var item in opponent.Backpack)
                {
                    game.World[opponent.PosX, opponent.PosY].Contents.Add(item);
                }
                opponent.Backpack.Clear();

                opponent.PosX = random.Next(game.World.GetLength(0));
                opponent.PosY = random.Next(game.World.GetLength(1));
                opponent.Health = opponent.MaxHealth;

                var dbs = new StatisticDbContext();
                var opponentStats = dbs.Statistics.Find(opponent.PlayerId);
                var playerStats = dbs.Statistics.Find(player.PlayerId);

                opponentStats.Deaths += 1;
                playerStats.Kills += 1;

                dbs.SaveChanges();

                return true;
            }
            return false;

        }
    }
}
