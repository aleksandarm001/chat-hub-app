namespace backend.Models
{
    public class Message
    {
        public long? Id { get; set; }
        public long SenderId { get; set; }
        public long RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }

        public Message()
        {
            Timestamp = DateTime.UtcNow;
            IsRead = false; 
        }

        public Message(long senderId, long recipientId, string content)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Content = content;
            Timestamp = DateTime.UtcNow; 
            IsRead = false; 
        }

        public Message(long id, long senderId, long recipientId, string content, DateTime timestamp, bool isRead)
        {
            Id = id;
            SenderId = senderId;
            RecipientId = recipientId;
            Content = content;
            Timestamp = timestamp;
            IsRead = isRead;
        }
    }
}