using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities.ChatRooms
{
    public class ChatRoomParticipant
    {
        public ObjectId UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
