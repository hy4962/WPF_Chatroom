using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_ChatRoom.ViewModels;
namespace WPF_ChatRoom.Models;

public class ChatMessageEntity : ViewModelBase
{
    private string _senderEndPoint;
    private string _content;

    public int Id { get; set; }

    public string SenderEndPoint
    {
        get => _senderEndPoint;
        set { _senderEndPoint = value; OnPropertyChanged(); }
    }

    public string Content
    {
        get => _content;
        set { _content = value; OnPropertyChanged(); }
    }

    public bool IsSelf { get; set; }
    public DateTime CreatedAt { get; set; }

}