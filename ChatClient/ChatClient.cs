using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("====================================");
            var client = new ChatLibrary.ChatClient();

            Console.WriteLine("<Please enter your name...>");
            var name = Console.ReadLine();

            var succeed = client.Connect("127.0.0.1", 4099);

            if (!succeed)
            {
                return;
            }

            client.SetName(name);
            Console.WriteLine("<You can press any key to start entering text...>");

            while (true)
            {
                var msg = Console.ReadLine();

                if (msg == "exit")
                {
                    Console.WriteLine("<Bye...>");
                    client.Disconnect();
                    break;
                }

                client.SendMessage(msg);
                Console.WriteLine("<Message sent, press any key to start entering text again...>");
            }
        }
    }
}
