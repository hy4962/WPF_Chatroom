namespace WPF_ChatRoom.Models;

public class SettingsModel
{
    private static readonly Lazy<SettingsModel> _lazy = new Lazy<SettingsModel>(() => new SettingsModel());//这样写的单例是延迟加载，真正被访问的时候才会new
        

    
    private string _serverIP;
    private int _serverPort;
    private int _serverMaxNumber;

    private string _clientIP;
    private int _clientPort;
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
    
    public string ClientIP
    {
        get=> _clientIP;
        set
        {            
            if (value == _clientIP) return;
            _clientIP = value;
        }
    }
    
    public int ClientPort
    {
        get=> _clientPort;
        set
        {            
            if (value == _clientPort) return;
            _clientPort = value;
        }
    }
    
    private SettingsModel()
    {
        ServerIP = "127.0.0.1";
        ServerPort = 8000;
        ServerMaxNumber = 10;
        ClientIP = "127.0.0.1";
        ClientPort = 8001;
    }

    public static SettingsModel Instance => _lazy.Value;
}