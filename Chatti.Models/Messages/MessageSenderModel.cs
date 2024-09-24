namespace Chatti.Models.Messages
{
    public class MessageSenderModel
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public string? FullName { get; set; }
    }
}
