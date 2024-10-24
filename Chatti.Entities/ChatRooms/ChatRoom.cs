using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities.ChatRooms
{
    public class ChatRoom : EntityBase
    {
        public required string Name { get; set; }
        public string? About { get; set; }
        public IList<ChatRoomParticipant> Participants { get; set; } = new List<ChatRoomParticipant>();
        public required ObjectId TenantId { get; set; } = new ObjectId();
        public ChatRoomSettings? Settings { get; set; }
    }
}
