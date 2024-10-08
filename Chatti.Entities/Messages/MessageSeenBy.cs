using MongoDB.Bson;

namespace Chatti.Entities.Messages
{
    public class MessageSeenBy : EntityBase
    {
        public ObjectId UserId { get; set; }
        public DateTime SeenAt { get; set; }
    }
}
