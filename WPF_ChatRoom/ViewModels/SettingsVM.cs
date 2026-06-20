using System.Windows;
using System.Windows.Input;
using WPF_ChatRoom.Models;
using WPF_ChatRoom.Sockets;

namespace WPF_ChatRoom.ViewModels;

internal class SettingsVM:ViewModelBase
{
    private static SettingsVM _instance = new SettingsVM(); 
    private SettingsModel _settingsModel = SettingsModel.Instance;//获取的是单例模式的settingmodel；
    private ServerManager _serverManager = ServerManager.Instance;//服务端侧功能的实例
    private ClientManager _clientManager = ClientManager.Instance;//客户端侧功能的实例
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
    public string RemoteServerIP
    {
        get => _settingsModel.ClientIP;
        set=>_settingsModel.ClientIP = value;
            
    }
    public int RemoteServerProt
    {
        get => _settingsModel.ClientPort;
        set=> _settingsModel.ClientPort = value;
    }
    public ICommand CreateServerCommand { get; }
    public ICommand ClientConnectCommand { get; }
    
    public static SettingsVM Instance
    {
        get
        {
            return _instance;
        }
    }
    private SettingsVM()
    {
        CreateServerCommand = new RelayCommand(_ => CreateServer_Executed());
        ClientConnectCommand= new RelayCommand(_ => ClientConnect_Executed());
    }

    /// <summary>
    /// 创建服务器
    /// </summary>
    private void CreateServer_Executed()
    {
        if (_serverManager.ServerDictionary.ContainsKey($"{ServerIP}:{ServerPort}")) //检测是否已经包含了需要创建的key，包含的话直接返回，否则重复创建抛异常
        {
            MessageBox.Show("已经成功创建过了，别点了");
            return;
        }
        _serverManager.CreationServer(ServerIP, ServerPort);
        _serverManager.ServerStart(ServerIP, ServerPort);
    }

    private void ClientConnect_Executed()
    {
        _clientManager.ClientConnect(RemoteServerIP,RemoteServerProt);
    }
    
    
}