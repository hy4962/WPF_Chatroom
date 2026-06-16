using System.Windows;
using System.Windows.Input;
using WPF_ChatRoom.Models;
using WPF_ChatRoom.Sockets;

namespace WPF_ChatRoom.ViewModels;

internal class SettingsVM:ViewModelBase
{
    private SettingsModel _settingsModel = SettingsModel.Instance;//获取的是单例模式的settingmodel；
    private SocketManager _socketManager = SocketManager.Instance;
    public string ServerIP
    {
        get=>_settingsModel.ServerIP;
        set=>_settingsModel.ServerIP = value;
    }    
    public int ServerPort
    {
        get=> _settingsModel.ServerPort;
        set=>_settingsModel.ServerPort = value;
    }
    public int ServerMaxNumber
    {
        get => _settingsModel.ServerMaxNumber;
        set=> _settingsModel.ServerMaxNumber = value;
    }
    public string ClientIP
    {
        get => _settingsModel.ClientIP;
        set=>_settingsModel.ClientIP = value;
            
    }
    public int ClientProt
    {
        get => _settingsModel.ClientPort;
        set=> _settingsModel.ClientPort = value;
    }
    public ICommand CreateServerCommand { get; }
    public SettingsVM()
    {
        CreateServerCommand = new RelayCommand(_ => CreateServer_Executed());
    }

    private void CreateServer_Executed()
    {
        _socketManager.CreationServer(ServerIP, ServerPort);
        _socketManager.ServerStart(ServerIP, ServerPort);
        _socketManager.MessageReceived += (sender, message, isSelf) =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                HomeVM.Instance.Messages.Add(new ChatMessage { Content = message, IsSelf = isSelf });
            });
        };
    }
}