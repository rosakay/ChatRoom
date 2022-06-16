using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Server
    {
        static HashSet<TcpClient> clients = new HashSet<TcpClient>();

        public static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            var server = new ChatLibrary.ChatServer();
            server.Bind(4099);
            server.Start();
        }
    }
}
