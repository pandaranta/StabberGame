using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Stabber;
using Newtonsoft.Json;


namespace StabberServer
{
    class Program
    {
        const String IPADDRESS = "127.0.0.1";
        //const String IPADDRESS = "10.56.5.232";
        const Int32 PORT = 8001;
        static IPAddress ipAddress = IPAddress.Parse(IPADDRESS);
        static IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);
        static UTF8Encoding encoding = new UTF8Encoding();
        private static Socket socket = null;
        private static Socket listeningSocket = null;

        const int BUFFERLENGTH = 100;
        static Room[,] world;

        static WorldDbContext dbw = new WorldDbContext();

        static void Main(string[] args)
        {
            var dbworld = new World();
            dbw.Worlds.Add(dbworld);
            dbw.SaveChanges();

            GenerateWorld();
            PersistWorldInDb();

            listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(localEndPoint);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Listen(100);

            while (true)
            {
                socket = listeningSocket.Accept();
                Thread thread = new Thread(Verifying);
                thread.Start(socket);
            }
        }
        static void PersistWorldInDb()
        {
            var temp = dbw.Worlds.Find(1);

            string jsonWorld = JsonConvert.SerializeObject(world);

            temp.JsonWorld = jsonWorld;

            dbw.SaveChanges();

        }

        static void GenerateWorld()
        {
            world = new Room[10, 10];

            for (int i = 0; i < world.GetLength(0); i++)
            {
                for (int j = 0; j < world.GetLength(1); j++)
                {
                    world[i, j] = new Room();
                }
            }
        }

        private static void Verifying(object socketIn)
        {
            Socket socket = (Socket)socketIn;
            Byte[] bufferIn = new Byte[BUFFERLENGTH];

            while (true)
            {
                socket.Receive(bufferIn);
                string response = string.Empty;
                bool loggedIn = false;
                string requestString = encoding.GetString(bufferIn).TrimEnd('\0').Trim();
                String[] tokens = requestString.Split(' ');
                if (tokens[0] != "DDO/1.0")
                {
                    response = "DDO/1.0 ERROR Use DDO/1.0 protocol";
                }
                else
                {
                    switch (tokens[1])
                    {
                        case "LOGIN":
                            {
                                UserDbContext playerDB = new UserDbContext();
                                var userName = tokens[2];

                                var password = tokens[3];
                                User player = playerDB.Users.Where(p => p.UserName == userName).Single();
                                var responseLocal = string.Empty;
                                if (player != null)
                                {
                                    if (player.Password == password)
                                    {
                                        responseLocal = $"DDO/1.0 LOGIN ACCEPTED";
                                        loggedIn = true;
                                    }
                                    else
                                    {
                                        responseLocal = $"DDO/1.0 LOGIN REJECTED Wrong password";
                                    }
                                }
                                else
                                {
                                    responseLocal = $"DDO/1.0 LOGIN REJECTED Player doesnt exist.";
                                }
                            }
                            break;
                        case "MOVE":
                            if(loggedIn == false)
                            {
                                response = "DDO/1.0 LOGIN REJECTED";
                            }
                            else
                            {
                                
                                
                                switch (tokens[2])
                                {
                                    case ("Up"):
                                        //do something with player
                                        ;
                                        break;
                                    case ("Down"):
                                        //do something with player
                                        ;
                                        break;
                                    case ("Right"):
                                        //do something with player
                                        ;
                                        break;
                                    case ("Left"):
                                        //do something with player
                                        ;
                                        break;
                                    default:
                                        break;
                                };
                            }			
                            break;
                        case "GETSTATE": string json = JsonConvert.SerializeObject(world); response = $"DDO/1.0  SENDSTATE {json}";
                             break;
                            
                        default: break;
                    }
                }
                Byte[] bufferOut = encoding.GetBytes(response);
                socket.Send(bufferOut);
            }
        }

        static string CheckLogIn(string[] tokens)
        {
            UserDbContext playerDB = new UserDbContext();
            var userName = tokens[2];
            
            var password = tokens[3];
            User player = playerDB.Users.Where(p => p.UserName == userName).Single();
            var responseLocal = string.Empty;
            if(player != null)
            {
                if(player.Password == password)
                {
                    responseLocal = $"DDO/1.0 LOGIN ACCEPTED";
                }
                else
                {
                    responseLocal = $"DDO/1.0 LOGIN REJECTED Wrong password";
                }
            }
            else
            {
                responseLocal = $"DDO/1.0 LOGIN REJECTED Player doesnt exist.";
            }

            return responseLocal;
        }

        //static Boolean ReceiveRequest(Socket socket, String opponent,
        //    out Int32 row, out Int32 column)
        //{
        //    row = -1; column = -1;
        //    Int32 threadId = Thread.CurrentThread.ManagedThreadId;
        //    Byte[] bufferIn = new Byte[BUFFERLENGTH];
        //    socket.Receive(bufferIn);
        //    String request = encoding.GetString(bufferIn).TrimEnd('\0').Trim();
        //    Console.WriteLine(threadId + ": Received request from " + socket.RemoteEndPoint + ": " + request);
        //    String[] tokens = request.Split(' ');
        //    Boolean ok = tokens.Length == 5 &&
        //        tokens[0] == "BAP/1.1" && tokens[1] == "SHOT" &&
        //        Int32.TryParse(tokens[2], out row) &&
        //        row >= 0 && row < BOARDSIDE &&
        //        Int32.TryParse(tokens[3], out column) &&
        //        column >= 0 && column < BOARDSIDE &&
        //        tokens[4] == opponent;
        //    return ok;
        //}

        //static void SendResponse(Socket socket, Boolean ok, Square[,] board,
        //    Int32 row, Int32 column, String opponent)
        //{
        //    Int32 threadId = Thread.CurrentThread.ManagedThreadId;
        //    Boolean isHit = false;
        //    ok = ok && row >= 0 && row < BOARDSIDE
        //        && column >= 0 && column < BOARDSIDE;
        //    if (ok)
        //        isHit = board[row, column] == Square.SHIP;
        //    String response = null;
        //    if (ok)
        //        response = "BAP/1.1 HIT " + row + " " + column +
        //            " " + isHit.ToString().ToUpper() + " " + opponent;
        //    else
        //        response = "BAP/1.1 ERROR Bad request.";
        //    Byte[] bufferOut = encoding.GetBytes(response);
        //    socket.Send(bufferOut);
        //    Console.WriteLine(threadId + ": Sent response to " + socket.RemoteEndPoint + ": " + response);
        //}
    }
}