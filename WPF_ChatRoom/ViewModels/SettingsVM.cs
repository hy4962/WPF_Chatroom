using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        get => _settingsModel.RemoteServerIP;
        set=>_settingsModel.RemoteServerIP = value;
            
    }
    public int RemoteServerProt
    {
        get => _settingsModel.RemoteServerPort;
        set=> _settingsModel.RemoteServerPort = value;
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
        LoadJson();
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
        SaveJson();
    }

    /// <summary>
    /// 连接到服务器
    /// </summary>
    private void ClientConnect_Executed()
    {
        _clientManager.ClientConnect(RemoteServerIP,RemoteServerProt);
        SaveJson();
    }

    private void SaveJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,           // 格式化缩进，方便阅读
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // 驼峰命名（可选）
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // 忽略null值
        };
        string json_string = JsonSerializer.Serialize(_settingsModel,options);
        File.WriteAllText("Setting.json", json_string,Encoding.UTF8);
    }

    private void LoadJson()
    {
        if (!File.Exists($"Setting.json"))
            return;
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // 和保存时保持一致
        };
        string json_string = File.ReadAllText("Setting.json", Encoding.UTF8);
        SettingsModel settings = JsonSerializer.Deserialize<SettingsModel>(json_string, options);
        _settingsModel = settings;
    }
    
}