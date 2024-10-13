using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.ChatRooms
{
    public class ChatRoomRequestModel
    {
        public required string Name { get; set; }
        public List<string> Participants { get; set; } = new();
    }
}
