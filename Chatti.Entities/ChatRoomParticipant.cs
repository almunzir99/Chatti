using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class ChatRoomParticipant : EntityBase
    {
        public ObjectId ChatRoomId { get; set; }
        public ObjectId UserId { get; set; }
        public bool IsOwner { get; set; }
    }
}
