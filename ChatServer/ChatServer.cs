using System;

namespace ChatServer
{
    internal class ChatServer
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            var server = new ChatLibrary.ChatServer();
            server.Bind(4099);
            server.Start();
        }
    }
}
