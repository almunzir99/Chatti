using MongoDB.Bson;

namespace Chatti.Entities.Messages
{
    public class Message : EntityBase
    {
        public required string Content { get; set; } = string.Empty;
        public required ObjectId ChatRoomId { get; set; }
        public required MessageSender Sender { get; set; }
        public MessageAttachment? Attachment { get; set; }
        public List<MessageSeenBy> seenBy = new();

    }
}
