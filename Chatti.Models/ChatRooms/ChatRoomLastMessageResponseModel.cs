namespace Chatti.Models.ChatRooms
{
    public class ChatRoomLastMessageResponseModel
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
        public string? Sender { get; set; }

    }
}
