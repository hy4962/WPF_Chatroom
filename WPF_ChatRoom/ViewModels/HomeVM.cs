using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WPF_ChatRoom.Models;
using WPF_ChatRoom.Sockets;

namespace WPF_ChatRoom.ViewModels
{
    internal class HomeVM:ViewModelBase
    {

        public ObservableCollection<ChatMessage> Messages { get; } = new();
        private SocketManager _socketManager;

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        public ICommand SendCommand { get; }

        public HomeVM()
        {
            SendCommand = new RelayCommand(_ => SendMessage());
            Messages.Add(new ChatMessage { Content = "你好", IsSelf = false });
            InitSocketManager();
        }

        private void InitSocketManager()
        {
            _socketManager = new SocketManager();
            _socketManager.CreationServer("127.0.0.1", 8080);
            _socketManager.ServerStart("127.0.0.1", 8080);
            _socketManager.MessageReceived += (sender, message, isSelf) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add(new ChatMessage { Content = message, IsSelf = isSelf });
                });
            };
        }

        private void SendMessage()
        {
            if (!string.IsNullOrEmpty(InputText))
            {
                Messages.Add(new ChatMessage { Content = InputText, IsSelf = true });
                _socketManager.ClientManager.Values.ToList().ForEach(client =>
                {
                    byte[] data = Encoding.UTF8.GetBytes(InputText);
                    client.Send(data);
                });
                InputText = "";
            }
        }
       


    }
}
