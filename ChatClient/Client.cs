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

            var succeed = client.Connect("127.0.0.1", 4099);

            if (!succeed)
            {
                return;
            }

            Console.WriteLine("<You can press any key to start entering text...>");

            while (true)
            {
                var msg = Console.ReadLine();

                if (msg.ToLower() == "exit")
                {
                    Console.WriteLine("<Bye...>");
                    client.Disconnect();
                    break;
                }

                client.SendData(msg);
                Console.WriteLine("<Message sent, press any key to start entering text again...>");
            }
        }
    }
}
