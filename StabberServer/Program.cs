using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StabberServer
{
    class Program
    {
        static void Main(string[] args)
        {

           

        }

        static Boolean ReceiveRequest(Socket socket, String opponent,
            out Int32 row, out Int32 column)
        {
            row = -1; column = -1;
            Int32 threadId = Thread.CurrentThread.ManagedThreadId;
            Byte[] bufferIn = new Byte[BUFFERLENGTH];
            socket.Receive(bufferIn);
            String request = encoding.GetString(bufferIn).TrimEnd('\0').Trim();
            Console.WriteLine(threadId + ": Received request from " + socket.RemoteEndPoint + ": " + request);
            String[] tokens = request.Split(' ');
            Boolean ok = tokens.Length == 5 &&
                tokens[0] == "BAP/1.1" && tokens[1] == "SHOT" &&
                Int32.TryParse(tokens[2], out row) &&
                row >= 0 && row < BOARDSIDE &&
                Int32.TryParse(tokens[3], out column) &&
                column >= 0 && column < BOARDSIDE &&
                tokens[4] == opponent;
            return ok;
        }

        static void SendResponse(Socket socket, Boolean ok, Square[,] board,
            Int32 row, Int32 column, String opponent)
        {
            Int32 threadId = Thread.CurrentThread.ManagedThreadId;
            Boolean isHit = false;
            ok = ok && row >= 0 && row < BOARDSIDE
                && column >= 0 && column < BOARDSIDE;
            if (ok)
                isHit = board[row, column] == Square.SHIP;
            String response = null;
            if (ok)
                response = "BAP/1.1 HIT " + row + " " + column +
                    " " + isHit.ToString().ToUpper() + " " + opponent;
            else
                response = "BAP/1.1 ERROR Bad request.";
            Byte[] bufferOut = encoding.GetBytes(response);
            socket.Send(bufferOut);
            Console.WriteLine(threadId + ": Sent response to " + socket.RemoteEndPoint + ": " + response);
        }
    }
}
