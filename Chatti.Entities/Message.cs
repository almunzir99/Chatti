using MongoDB.Bson;

namespace Chatti.Entities
{
    public class Message : EntityBase  
    {
        public required string Content { get; set; } = String.Empty;
        public required ObjectId ChatRoomId { get; set; }
        public required MessageSender Sender { get; set; }
        public MessageAttachment? Attachment { get; set; }
    }
}
