using MongoDB.Bson;

namespace Chatti.Entities
{
    public class Message : EntityBase
    {
        public string? Content { get; set; }
        public required User SenderId { get; set; }
        public required ObjectId ChatRoomId { get; set; }
        public Attachment? Attachment { get; set; }

    }
}
