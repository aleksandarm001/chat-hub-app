namespace backend.Models;

public class Chat
{
    public long? Id { get; set; }
    public long UserId1 { get; set; }
    public long UserId2 { get; set; }
    public string Messages { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Chat()
    {
    }

    public Chat(long userId1, long userId2)
    {
        UserId1 = userId1;
        UserId2 = userId2;
        CreatedAt = DateTime.UtcNow; 
    }

    public Chat(long id, long userId1, long userId2, DateTime createdAt, List<Message> messages)
    {
        Id = id;
        UserId1 = userId1;
        UserId2 = userId2;
        CreatedAt = createdAt;
    }
}