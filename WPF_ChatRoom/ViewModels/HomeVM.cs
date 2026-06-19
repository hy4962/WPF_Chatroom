using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF_ChatRoom.Models;
using WPF_ChatRoom.Sockets;

namespace WPF_ChatRoom.ViewModels
{
    internal class HomeVM:ViewModelBase
    {
        private static HomeVM _instance;
        private static readonly object _lock = new object();//这个是ai告诉我这么写的...锁对象 _lock，作用：解决多线程并发问题。如果多个线程同时调用 Instance，没有锁的话，可能同时判断 _instance == null，导致创建多个实例。为什么用 readonly？锁对象本身不需要修改，readonly 保证它不会被意外替换，更安全。

        public ObservableCollection<ChatMessage> Messages { get; } = new();
        private ServerManager _serverManager = ServerManager.Instance;
        private ClientManager _clientManager = ClientManager.Instance;


        
        public ObservableCollection<string> ConnectClients { get; } = new();
        public ObservableCollection<string> RemoteServers { get; } = new();
        public string SelectConnectClient { get; set; }
        public string SelectRemoteServer { get; set; }


        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        public ICommand SendCommand { get; }

        
        public static HomeVM Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)_instance = new HomeVM();
                    return _instance;
                }
            }
        }
        
        private HomeVM()
        {
            SendCommand = new RelayCommand(_ => SendMessage());
            Messages.Add(new ChatMessage { Content = "这是一条测试消息", IsSelf = false });
            _serverManager.AddClient += (string EndPoint) =>
            {
                //Application.Current.Dispatcher.Invoke回到ui线程去操作，因为通知的时候是在后台线程(Task)里
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ConnectClients.Add(EndPoint);
                });
            };
            _clientManager.AddServer += (string EndPoint) =>
            {
                //Application.Current.Dispatcher.Invoke回到ui线程去操作，因为通知的时候是在后台线程(Task)里
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RemoteServers.Add(EndPoint);
                });
            };
        }




        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (SelectConnectClient != null)
                {
                    _serverManager.Send(SelectConnectClient, InputText);
                }
        
                if (SelectRemoteServer != null)
                {
                    _clientManager.Send(SelectRemoteServer, InputText);
                }
                _clientManager.Send(SelectRemoteServer, InputText);
                Messages.Add(new ChatMessage { Content = InputText, IsSelf = true });
            }
        }
       


    }
}
