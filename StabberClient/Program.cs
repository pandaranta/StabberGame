using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stabber;

namespace StabberClient
{
    class Program
    {
        const string IPADDRESS = "127.0.0.1";
        //const String IPADDRESS = "10.56.5.232";
        const int PORT = 8001;
        static IPAddress ipAddress = IPAddress.Parse(IPADDRESS);
        static IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, PORT);
        static UTF8Encoding encoding = new UTF8Encoding();
        static Socket socket = null;
        const int bufferLength = 100;
        static byte[] bufferIn = new byte[100];
        static string response = string.Empty;

        static void Main(string[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(remoteEndPoint);

            Login();
            GetState();



            //switch (tokens[1])
            //{
            //    case "LOGIN":; break;
            //    case "SENDSTATE": var world = JsonConvert.DeserializeObject(tokens[2]); break;
            //    default:
            //        break;
            //}
            //Console.WriteLine("Recieved message: " + messageIn);
            //Console.ReadLine();
            //socket.Close();
        }

        static void GetState()
        {
            string response = $"DDO/1.0 GETSTATE";

            byte[] bufferOut = new byte[bufferLength];
            bufferOut = Encoding.Default.GetBytes(response);
            socket.Send(bufferOut);

            socket.Receive(bufferIn);

            string messageIn = encoding.GetString(bufferIn).TrimEnd('\0').Trim();
            string[] tokens = messageIn.Split(' ');

            if (tokens[1] == "SENDSTATE")
            {
                string jsonWorld = tokens[2];

                object jsonObject = JsonConvert.DeserializeObject(jsonWorld);

                Room[,] world = (Room[,])jsonObject;


                for (int i = 0; i < world.GetLength(0); i++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;

                    for (int j = 0; j < world.GetLength(1); j++)
                    {
                        //if (i == player1.PosX && j == player1.PosY)
                        //{
                        //    Console.ForegroundColor = ConsoleColor.Blue;
                        //    Console.Write("P1");
                        //    Console.ForegroundColor = ConsoleColor.Black;
                        //}
                        //else if (i == player2.PosX && j == player2.PosY)
                        //{
                        //    Console.ForegroundColor = ConsoleColor.Blue;
                        //    Console.Write("P2");
                        //    Console.ForegroundColor = ConsoleColor.Black;
                        //}
                        if (world[i, j].HasItem())
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("o ");
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else { Console.Write("  "); }
                    }

                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine();

                    Console.ReadKey();

                }
            }
        }

        static void Login()
        {
            bool isLoggedin = false;
            string message = string.Empty;

            while (!isLoggedin)
            {
                Console.Clear();
                Console.WriteLine(message);
                Console.WriteLine("Enter username: ");
                string userName = Console.ReadLine();
                Console.WriteLine("Enter password: ");
                string password = Console.ReadLine();

                string response = $"DDO/1.0 LOGIN {userName} {password}";

                byte[] bufferOut = new byte[bufferLength];
                bufferOut = Encoding.Default.GetBytes(response);
                socket.Send(bufferOut);

                socket.Receive(bufferIn);
                string messageIn = encoding.GetString(bufferIn).TrimEnd('\0').Trim();
                string[] tokens = messageIn.Split(' ');

                switch (tokens[2])
                {
                    case "ACCEPTED": isLoggedin = true; break;
                    case "REJECTED": message = messageIn; break;

                    default:
                        message = "im a newb";
                        break;
                }
            }


        }
    }
}
