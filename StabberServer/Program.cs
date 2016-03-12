using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
		static string response = string.Empty;

		static void Main(string[] args)
		{
			listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listeningSocket.Bind(localEndPoint);

			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listeningSocket.Listen(100);
			Byte[] bufferIn = new Byte[BUFFERLENGTH];

			while (true)
			{
				socket = listeningSocket.Accept();
				socket.Receive(bufferIn);
				Thread thread = new Thread(Verifing);
				thread.Start(bufferIn);
			}

		}

		private static void Verifing(object requestIn)
		{
			byte[] request = (byte[])requestIn;
			string requestString = encoding.GetString(request);
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
						CheckLogIn(tokens);
						break;
					case "MOVE":
						switch (tokens[2])
						{
							case ("Up"):
								;
								break;
							case ("Down"):
								;
								break;
							default:
								break;
						};
						break;
					case "GETSTATE":; break;
					default: break;
				}
			}
			Byte[] bufferOut = encoding.GetBytes(response);
			socket.Send(bufferOut);
		}

		static void CheckLogIn(string[] tokens)
		{
			PlayerDbContext playerDB = new PlayerDbContext();

			foreach (var item in playerDB.Players)
			{
				string username = item.PlayerName;
				if (username == tokens[2])
				{
					string password = item.Password;
					if (password == tokens[3])
					{
						response = $"DDO/1.0 LOGIN ACCEPTED {tokens[2]}";
					}
					else
					{
						response = $"DDO/1.0 LOGIN REJECTED {tokens[2]} wrong password";
					}
				}
				else
					response = $"DDO / 1.0  LOGIN REJECTED {tokens[2]} DOES NOT EXIST";
			}
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