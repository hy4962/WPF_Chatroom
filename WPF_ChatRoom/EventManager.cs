namespace WPF_ChatRoom;

public static class EventManager
{
    public static event Action<string, string,bool> OnMessageReceived;
    public static event  Action<string> OnConnectClient;
    public static event Action<string> OnRemoteServer;
    public static void RaiseMessageReceived(string sender, string message, bool IsSelf)
    {
        OnMessageReceived?.Invoke(sender, message, IsSelf);
    }

    public static void RaiseConnectClient(string EndPoint)
    {
        OnConnectClient?.Invoke(EndPoint);
    }

    public static void RaiseRemoteServer(string EndPoint)
    {
        OnRemoteServer?.Invoke(EndPoint);
    }

}