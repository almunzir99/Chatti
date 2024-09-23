using Chatti.Models.Users;

namespace Chatti.Models.Messages
{
    public class MessageResponseModel
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public UserResponseModel? Sender { get; set; }
        public string? ChatRoomId { get; set; }
        public MessageAttachmentModel? Attachment { get; set; }
        public DateTime SentAt { get; set; }

    }
}
