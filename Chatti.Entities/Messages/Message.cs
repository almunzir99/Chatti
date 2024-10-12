using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chatti.Entities.Messages
{
    public class Message : EntityBase
    {
        public required string Content { get; set; } = string.Empty;
        public required ObjectId ChatRoomId { get; set; }
        public required MessageSender Sender { get; set; }
        public MessageAttachment? Attachment { get; set; }
        [BsonElement("SeenBy")]
        public List<MessageSeenBy> SeenBy { get; set; } = new();

    }
}
