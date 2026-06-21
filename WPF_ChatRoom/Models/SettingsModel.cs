using System.Text.Json.Serialization;

namespace WPF_ChatRoom.Models;

public class SettingsModel
{
    private static readonly Lazy<SettingsModel> _lazy = new Lazy<SettingsModel>(() => new SettingsModel());//这样写的单例是延迟加载，真正被访问的时候才会new
    
    private string _serverIP;
    private int _serverPort;
    private int _serverMaxNumber;

    private string _remoteServerIP;
    private int _remoteServerPort;
    private static readonly object _lock = new object();//项目里目前唯一看不懂的玩意？？这特么啥玩意啊

    public string ServerIP
    {
        get=> _serverIP;
        set
        {
            if (value == _serverIP) return;
            _serverIP = value;
        }
    }
    
    public int ServerPort
    {
        get=> _serverPort;
        set
        {
            if (value == _serverPort) return;
            _serverPort = value;
        }
    }

    public int ServerMaxNumber
    {
        get=> _serverMaxNumber;
        set
        {
            if (value == _serverMaxNumber) return;
            _serverMaxNumber = value;
        }
    }
    
    public string RemoteServerIP
    {
        get=> _remoteServerIP;
        set
        {            
            if (value == _remoteServerIP) return;
            _remoteServerIP = value;
        }
    }
    
    public int RemoteServerPort
    {
        get=> _remoteServerPort;
        set
        {            
            if (value == _remoteServerPort) return;
            _remoteServerPort = value;
        }
    }
    
    [JsonConstructor]//这个是告诉Json序列器用这个构造函数来创建对象
    private SettingsModel()
    {
    }

    public static SettingsModel Instance => _lazy.Value;
}