namespace WPF_ChatRoom;

public static class EventManager
{
    public static event Action<string, string,bool>? OnMessageReceived;//收到消息
    public static event Action<string>? OnClientConnect; //有新的客户端接入
    public static event Action<string>? OnRemoteServer; //连接至服务端
    
    
    
    public static void RaiseMessageReceived(string sender, string message,bool isMe)
    {
        OnMessageReceived?.Invoke(sender,message,isMe);
    }
    
    public static void RaiseClientConnect(string EndPoint)
    {
        OnClientConnect?.Invoke(EndPoint);
    }
    
    public static void RaiseRemoteServer(string EndPoint)
    {
        OnRemoteServer?.Invoke(EndPoint);
    }
    
    
}