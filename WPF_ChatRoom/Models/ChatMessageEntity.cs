namespace WPF_ChatRoom.Models;

public class ChatMessageEntity
{
    public int Id { get; set; }
    public string SenderEndPoint { get; set; }
    public string Content { get; set; }
    public bool IsSelf { get; set; }
    public DateTime CreatedAt { get; set; }
}