using Microsoft.EntityFrameworkCore;

namespace WPF_ChatRoom.Models;

public class ChatDbContext : DbContext
{
    public DbSet<ChatMessageEntity> Message { get; set; }

    public ChatDbContext()
    {
        Database.EnsureCreated();//检测表格是否存在，不存在则创建一个，存在则无操作
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ChatRoom.db");
    }

    /// <summary>
    /// 将ChatMessage转换成数据库需要的实体表ChatMessageEntity
    /// </summary>
    /// <param name="msg"></param>
    /// <returns>ChatMessageEntity</returns>
    public ChatMessageEntity FormChatMessage(ChatMessage msg)
    {
        ChatMessageEntity Entity = new ChatMessageEntity();
        Entity.SenderEndPoint = msg.EndPoint;
        Entity.Content = msg.Content;
        Entity.IsSelf = msg.IsSelf;
        Entity.CreatedAt = DateTime.Now;
        return Entity;
    }

    public ChatMessage FormChatMessageEntity(ChatMessageEntity Entity)
    {
        ChatMessage msg = new ChatMessage(Entity.SenderEndPoint, Entity.Content, Entity.IsSelf);
        msg.Time = Entity.CreatedAt;//因为msg默认是取当前时间，需要用创建时间去覆盖
        return msg;
    }

    public void AddMessage(ChatMessageEntity Entity)
    {
        Message.Add(Entity);//往表格里面添加具体的实体
        SaveChanges();//保存数据库的更改
    }
    
}