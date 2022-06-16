using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public class ChatServer
    {
        private int m_port;
        private TcpListener m_listener;
        private Thread m_handleThread;
        private readonly Dictionary<string, TcpClient> m_clients = new Dictionary<string, TcpClient>();
        private readonly Dictionary<string, string> m_userNames = new Dictionary<string, string>();

        public ChatServer()
        {
        }

        public void Bind(int port)
        {
            m_port = port;

            m_listener = new TcpListener(IPAddress.Any, port);
            Console.WriteLine("Server start at port {0}", port);
            m_listener.Start();
        }

        public void Start()
        {
            m_handleThread = new Thread(ClientsHandler);
            m_handleThread.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection... ");
                var client = m_listener.AcceptTcpClient();

                var clientId = client.Client.RemoteEndPoint.ToString();
                Console.WriteLine("Client has connected from {0}", clientId);

                lock (m_clients)
                {
                    m_clients.Add(clientId, client);
                    m_userNames.Add(clientId, "Unknown");
                }
            }
        }

        private void ClientsHandler()
        {
            while (true)
            {
                lock (m_clients)
                {
                    foreach (var clientId in m_clients.Keys)
                    {
                        var client = m_clients[clientId];

                        try
                        {
                            if (client.Available > 0)
                            {
                                ReceiveMessage(clientId);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Client {0} Error: {1}", clientId, e.Message);
                        }
                    }
                }
            }
        }

        private void ReceiveMessage(string clientId)
        {
            var client = m_clients[clientId];
            var stream = client.GetStream();

            var numBytes = client.Available;
            var buffer = new byte[numBytes];
            var bytesRead = stream.Read(buffer, 0, numBytes);
            var request = System.Text.Encoding.Unicode.GetString(buffer);

            if (request.StartsWith("LOGIN:", StringComparison.OrdinalIgnoreCase))
            {
                var tokens = request.Split(':');
                m_userNames[clientId] = tokens[1];
                Console.WriteLine($"Client {m_userNames[clientId]} Login from {clientId}");
                return;
            }

            if (request.StartsWith("MESSAGE:", StringComparison.OrdinalIgnoreCase))
            {
                var tokens = request.Split(':');
                var message = tokens[1];
                Console.WriteLine($"Text: {message} from {m_userNames[clientId]}");
            }
        }
    }
}
