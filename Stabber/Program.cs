using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Stabber
{
    //Stabber main class.
   public class Game
    {
        
        public Room[,] World;
        Queue<Player> PlayerQueue { get; set; }

        static Random random = new Random();

        // Constructor
        public Game()
        {
            World = new Room[10,10];

            PlayerQueue = new Queue<Player>();
        }

        // Init and main loop
        static void Main(string[] args)
        {
            Game game = new Game();

            //This text will show while database is beeing setup.
            Console.WriteLine("Setting up some technical stuff and");            
            Console.WriteLine("Creating game..");

            // Create some database objects for later use.
            var db = new PlayerDbContext();
            var dbs = new StatisticDbContext();
            var dbw = new WorldDbContext();

            // Creating players.
            Player player1 = new Player(game.World.GetLength(0), game.World.GetLength(1), 1, "Player 1");
            Player player2 = new Player(game.World.GetLength(0), game.World.GetLength(1), 2, "Player 2");

            // Create some statistics objects to keep track of player stats.
            Statistic statsP1 = new Statistic();
            Statistic statsP2 = new Statistic();
            
            // Some random inits.
            string header = string.Empty;
            string footer = string.Empty;

            Console.SetWindowSize(35, 17);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            // Create the gameboard, que the players.
            game.GenerateWorld(game);
            game.PlayerQueue.Enqueue(player1);
            game.PlayerQueue.Enqueue(player2);

            // Serialize:ing the gameboard and making an entity containing the string.
            string jsonWorld = JsonConvert.SerializeObject(game.World);
            //World dbWorld = new World(jsonWorld);

            // Init two players from the queue.
            Player player = game.PlayerQueue.Dequeue();
            Player opponent = game.PlayerQueue.Peek();

            // Adding the players, the statistics and the world entitys to the databases.
            db.Players.Add(player1);
            db.Players.Add(player2);

            dbs.Statistics.Add(statsP1);
            dbs.Statistics.Add(statsP2);
            //dbw.Worlds.Add(dbWorld); 

            db.SaveChanges();
            dbs.SaveChanges();
            dbw.SaveChanges();

            // The infinite game loop.
            while (true)
            {
                Console.Clear();

                // This is the top of the console.
                game.PrintPlayerStats(player1, player2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(header);
                Console.ForegroundColor = ConsoleColor.Black;

                // this ends the top of the console.
                game.PrintWorld(game, player1, player2);

                // this is the bottom of the console.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(footer);
                Console.ForegroundColor = ConsoleColor.Black;

                header = string.Empty;
                footer = string.Empty;

                Console.Write($"Make your move {player.Name}: ");
                ConsoleKeyInfo key = Console.ReadKey(true);
                // This ends the bottom of the console.

                if (player.Move(game, player, key, opponent))
                {
                    if (player.CheckIfDeadRespawn(game, player, opponent))
                    {
                        header = "Killed and respawned";                        
                    }

                    var dbPlayer = db.Players.Find(player.PlayerId);

                    dbPlayer.Damage = player.Damage;
                    dbPlayer.Backpack = player.Backpack;
                    dbPlayer.Health = player.Health;
                    dbPlayer.MaxHealth = player.MaxHealth;
                    dbPlayer.PosX = player.PosX;
                    dbPlayer.PosY = player.PosY;                    

                    db.SaveChanges();

                    game.GenerateGoldNugget(game);
                    game.GenerateHealthPotion(game);
                    game.PlayerQueue.Enqueue(player);

                    player = game.PlayerQueue.Dequeue();
                    opponent = game.PlayerQueue.Peek();
                }
                else footer = "Cant move in that direction";




            }
        }

        // Prints the generated world on the console
        void PrintWorld(Game game, Player player1, Player player2)
        {
            for (int i = 0; i < game.World.GetLength(0); i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;

                for (int j = 0; j < game.World.GetLength(1); j++)
                {
                    if (i == player1.PosX && j == player1.PosY)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("P1");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if (i == player2.PosX && j == player2.PosY)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("P2");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if (game.World[i, j].HasItem())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("o ");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else { Console.Write("  "); }
                }

                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine();
            }
        }

        // Print player stats on console.
        void PrintPlayerStats(Player player1, Player player2)
        {
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Name: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(player1.Name);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" Health: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(player1.Health);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Gold: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(player1.Backpack.Count);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Name: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(player2.Name);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" Health: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(player2.Health);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Gold: ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(player2.Backpack.Count);

        }

        //Generates the rooms in the room array.
        void GenerateWorld(Game game)
        {
            for (int i = 0; i < game.World.GetLength(0); i++)
            {
                for (int j = 0; j < game.World.GetLength(1); j++)
                {
                    game.World[i, j] = new Room();
                }
            }

        }

        //Randomly add a gold nugget to the game world.
        void GenerateGoldNugget(Game game)
        {
            if (random.Next(101) > 90)
            {
                game.World[random.Next(game.World.GetLength(0)), random.Next(game.World.GetLength(1))].Contents.Add(new Gold());
            }
        }


        //Randomly add a Health potion to the game world.
        void GenerateHealthPotion(Game game)
        {
            if(random.Next(101) > 95)
            {
                game.World[random.Next(game.World.GetLength(0)), random.Next(game.World.GetLength(1))].Contents.Add(new HealthPotion());

            }

        }


    }
}
