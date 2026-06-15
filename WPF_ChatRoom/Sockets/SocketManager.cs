using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WPF_ChatRoom.Sockets
{
    internal class SocketManager
    {
        public event Action<string, string,bool> MessageReceived;

        private Dictionary<string,Socket> _serverManager;

		public Dictionary<string,Socket> ServerManager
        {
			get { return _serverManager; }
			set { _serverManager = value; }
		}

		private Dictionary<string,Socket> _clientManager;

		public Dictionary<string,Socket> ClientManager
        {
			get { return _clientManager; }
			set { _clientManager = value; }
		}


        public SocketManager()
        {
            _serverManager = new Dictionary<string, Socket>();
            _clientManager = new Dictionary<string, Socket>();
        }



        public void CreationServer(string ip,int port)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            server.Bind(new IPEndPoint(ipAddress,port));
            _serverManager.Add($"{ip}:{port}", server);
        }

        public void ServerStart(string ip, int port)
        {
            if (_serverManager.TryGetValue($"{ip}:{port}", out Socket server))
            {
                server.Listen(10);
                AcceptClients(server);
            }
        }

        private void AcceptClients(Socket server)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        Socket client = server.Accept();//阻塞等待客户端连接
                        string clientEndPoint = client.RemoteEndPoint.ToString();
                        _clientManager.Add(clientEndPoint, client);
                        Task.Run(() => ClientReceiveLoop(client));
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            });

        }

        private void ClientReceiveLoop(Socket client)
        {
            byte[] buffer = new byte[4096];
            try
            {
                while (true)
                {
                    int bytesRead = client.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string clientEndPoint = client.RemoteEndPoint.ToString();
                        bool isSelf = false;
                        MessageReceived?.Invoke(clientEndPoint, message, isSelf);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
