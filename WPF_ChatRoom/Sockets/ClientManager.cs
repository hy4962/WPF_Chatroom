using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WPF_ChatRoom.Sockets;

public class ClientManager
{
    private static readonly ClientManager _instance = new ClientManager();
    public static ClientManager Instance => _instance;
    
    private Dictionary<string, Socket> _serverDictionary;

    public event Action<string> AddServer; 
    
    
    public Dictionary<string, Socket> ServerDictionary
    {
        get => _serverDictionary;
        set => _serverDictionary = value;
    }

    private ClientManager()
    {
        _serverDictionary =new Dictionary<string, Socket>();
    }

    public void ClientConnect(string ip, int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ip,port);
        _serverDictionary.Add($"{ip}:{port}",socket);
        AddServer?.Invoke($"{ip}:{port}");
    }

    public void Send(string serverEndPoint,string message)
    {
        Socket socket = _serverDictionary[serverEndPoint];
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        socket.Send(bytes);
    }

    
}