using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chatti.Entities.Messages
{
    public class MessageSeenBy
    {
        [BsonElement("UserId")]
        public ObjectId UserId { get; set; }
        [BsonElement("SeenAt")]
        public DateTime SeenAt { get; set; } = DateTime.Now;
    }
}
