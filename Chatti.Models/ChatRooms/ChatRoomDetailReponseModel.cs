using Chatti.Models.Messages;
using Chatti.Models.Users;

namespace Chatti.Models.ChatRooms
{
    public class ChatRoomDetailReponseModel
    {
        public string? Id { get; set; }
        public required string Name { get; set; }
        public List<UserResponseModel> Participants { get; set; } = new();
        public ChatRoomSettingsResponseModel? Settings { get; set; }
        public List<MessageAttachmentModel> Attachments { get; set; } = new();
        public string? AdminId { get; set; }

    }
}
