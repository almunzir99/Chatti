using Chatti.Models.Users;

namespace Chatti.Models.Messages
{
    public class MessageSeenByResponse
    {
        public UserResponseModel? User { get; set; }
        public DateTime SeenAt { get; set; }
    }
}
