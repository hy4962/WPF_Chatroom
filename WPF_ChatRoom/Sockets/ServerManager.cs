using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WPF_ChatRoom.ViewModels;

namespace WPF_ChatRoom.Sockets
{
    internal class ServerManager
    {
        private static readonly ServerManager _instance =new();
        public static ServerManager Instance => _instance;

        private Dictionary<string,Socket> _serverDictionary;
        private Dictionary<string,Socket> _clientDictionary;
        

		public Dictionary<string,Socket> ServerDictionary
        {
			get { return _serverDictionary; }
			set { _serverDictionary = value; }
		}
        
		public Dictionary<string,Socket> ClientDictionary
        {
			get { return _clientDictionary; }
			set { _clientDictionary = value; }
		}

        

        private ServerManager()
        {
            _serverDictionary = new Dictionary<string, Socket>();
            _clientDictionary = new Dictionary<string, Socket>();
        }


        /// <summary>
        /// 创建socket并作为服务端
        /// </summary>
        /// <param name="ip">需要绑定的IP</param>
        /// <param name="port">以及端口</param>
        /// <returns></returns>
        public bool CreationServer(string ip,int port)
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            server.Bind(new IPEndPoint(ipAddress,port));
            bool isTrue =  _serverDictionary.TryAdd($"{ip}:{port}", server);
            return isTrue;
        }

        /// <summary>
        /// 服务端开始监听
        /// </summary>
        /// <param name="ip">需要开始服务的IP</param>
        /// <param name="port">以及端口</param>
        public void ServerStart(string ip, int port)
        {
            if (_serverDictionary.TryGetValue($"{ip}:{port}", out Socket server))
            {
                server.Listen(SettingsVM.Instance.ServerMaxNumber);
                AcceptClients(server);
            }
        }

        
        /// <summary>
        /// 循环接收进入连接至服务端的客户端
        /// </summary>
        /// <param name="server">服务端</param>
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
                        _clientDictionary.Add(clientEndPoint, client);
                        EventManager.RaiseConnectClient(clientEndPoint);
                        Task.Run(() => ClientReceiveLoop(client));
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// 循环接收消息
        /// </summary>
        /// <param name="client"></param>
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
                        EventManager.RaiseMessageReceived(clientEndPoint, message,false);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Send(string clientEndPoint,string message)
        {
            if (_clientDictionary.TryGetValue(clientEndPoint, out Socket client))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                client.Send(bytes);
            }
        }




    }
}
