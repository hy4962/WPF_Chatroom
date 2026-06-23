using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPF_ChatRoom.Models;
using WPF_ChatRoom.Sockets;

namespace WPF_ChatRoom.ViewModels
{
    internal class HomeVM:ViewModelBase
    {
        private static HomeVM _instance;
        private static readonly object _lock = new object();//这个是ai告诉我这么写的...锁对象 _lock，作用：解决多线程并发问题。如果多个线程同时调用 Instance，没有锁的话，可能同时判断 _instance == null，导致创建多个实例。为什么用 readonly？锁对象本身不需要修改，readonly 保证它不会被意外替换，更安全。

        //聊天界面需要的表格和属性
        public ObservableCollection<ChatMessage> Messages { get; } = new();
        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }
        private ServerManager _serverManager = ServerManager.Instance;
        private ClientManager _clientManager = ClientManager.Instance;

        //----------列表框需要的表格和属性------------
        public ObservableCollection<string> ConnectClients { get; } = new();
        public ObservableCollection<string> RemoteServers { get; } = new();
        public string SelectConnectClient { get; set; }
        public string SelectRemoteServer { get; set; }
        //----------列表框需要的表格和属性------------
        
        public ICommand SendCommand { get; }

        private ChatDbContext db=new ChatDbContext();
        
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
            LoadAllMessages();
            EventManager.OnMessageReceived += (string sender, string message, bool IsSelf) =>
            {
                Application.Current.Dispatcher.Invoke
                (
                    () => Messages.Add(new ChatMessage(sender, message, IsSelf))
                );
            };
            EventManager.OnConnectClient += (string EndPoint) =>
            {
                //Application.Current.Dispatcher.Invoke回到ui线程去操作，因为通知的时候是在后台线程(Task)里
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ConnectClients.Add(EndPoint);
                });
            };
            EventManager.OnRemoteServer += (string EndPoint) =>
            {
                //Application.Current.Dispatcher.Invoke回到ui线程去操作，因为通知的时候是在后台线程(Task)里
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RemoteServers.Add(EndPoint);
                });
            };
        }

        /// <summary>
        /// 从数据库加载消息显示到UI上
        /// </summary>
        public void LoadAllMessages()
        {
            // 从数据库查出所有记录（按时间正序，最早的在前）
            List<ChatMessageEntity> entities = db.Message
                .OrderBy(e => e.CreatedAt)
                .ToList();
            // 逐个翻译成 UI 模型，放入集合
            foreach (ChatMessageEntity entity in entities)
            {
                Messages.Add(db.FormChatMessageEntity(entity));
            }
        }



        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                if (SelectConnectClient != null)
                {
                    _serverManager.Send(SelectConnectClient, InputText);
                    ChatMessage msg = new ChatMessage(SelectConnectClient, InputText, true);
                    Messages.Add(msg);
                    ChatMessageEntity entity =  db.FormChatMessage(msg);
                    db.AddMessage(entity);
                }
                if (SelectRemoteServer != null)
                {
                    _clientManager.Send(SelectRemoteServer, InputText);
                    ChatMessage msg = new ChatMessage(SelectRemoteServer, InputText, false);
                    ChatMessageEntity entity =  db.FormChatMessage(msg);
                    db.AddMessage(entity);
                    Messages.Add(msg);
                }
            }
        }
        
       


    }
}
