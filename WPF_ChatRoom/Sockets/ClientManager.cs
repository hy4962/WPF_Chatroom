using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WPF_ChatRoom.Sockets;

public class ClientManager
{
    private static readonly ClientManager _instance = new ClientManager();
    public static ClientManager Instance => _instance;
    
    private Dictionary<string, Socket> _serverDictionary;
    
    public Dictionary<string, Socket> ServerDictionary
    {
        get => _serverDictionary;
        set => _serverDictionary = value;
    }

    private ClientManager()
    {
        _serverDictionary =new Dictionary<string, Socket>();
    }

    public async void ClientConnect(string ip, int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await socket.ConnectAsync(ip,port);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        _serverDictionary.Add($"{ip}:{port}",socket);
        EventManager.RaiseRemoteServer($"{ip}:{port}");
    }

    public void Send(string serverEndPoint,string message)
    {
        Socket socket = _serverDictionary[serverEndPoint];
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        socket.Send(bytes);
    }

    
}