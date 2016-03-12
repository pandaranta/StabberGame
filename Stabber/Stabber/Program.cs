using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stabber
{
    //Stabber main class.
    class Game
    {
        public Room[,] World { get; set; }
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
            Player player1 = new Player(game.World.GetLength(0), game.World.GetLength(1), "Player 1");
            Player player2 = new Player(game.World.GetLength(0), game.World.GetLength(1), "Player 2");
            string header = string.Empty;
            string footer = string.Empty;
                        
            Console.SetWindowSize(35,17);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            game.GenerateWorld(game);
            game.PlayerQueue.Enqueue(player1);
            game.PlayerQueue.Enqueue(player2);

            Player player = game.PlayerQueue.Dequeue();
            Player opponent = game.PlayerQueue.Peek();           

            while (true)
            {
                Console.Clear();

                game.PrintPlayerStats(player1, player2);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(header);
                Console.ForegroundColor = ConsoleColor.Black;

                game.PrintWorld(game, player1, player2);
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(footer);                
                Console.ForegroundColor = ConsoleColor.Black;

                header = string.Empty;
                footer = string.Empty;

                Console.Write($"Make your move {player.Name}: ");
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (player.Move(game, player, key, opponent))
                {
                    if(player.CheckIfDeadRespawn(game, player, opponent))
                    {
                        header = "Killed and respawned";
                    }

                    game.GenerateGoldNugget(game);
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
            if (random.Next(101)>90)
            {
                game.World[random.Next(game.World.GetLength(0)), random.Next(game.World.GetLength(1))].Contents.Add(new Gold());
            }
        }


    }
}
