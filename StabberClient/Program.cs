using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StabberClient
{
	class Program
	{
		const String IPADDRESS = "127.0.0.1";
		//const String IPADDRESS = "10.56.5.232";
		const Int32 PORT = 8001;
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
			Console.WriteLine("Enter username: ");
			string userName = Console.ReadLine();
			Console.WriteLine("Enter password: ");
			string password = Console.ReadLine();

			string response = $"DDO/1.0 LOGIN {userName} {password} ";

			Byte[] bufferOut = new Byte[bufferLength];
			bufferOut = Encoding.Default.GetBytes(response);
			socket.Send(bufferOut);

			socket.Receive(bufferIn);
			string messageIn = encoding.GetString(bufferIn);
			Console.WriteLine("Recieved message: " + messageIn);
			Console.ReadLine();
			socket.Close();
		}
	}
}
