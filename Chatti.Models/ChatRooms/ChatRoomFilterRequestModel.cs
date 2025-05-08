using Chatti.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.ChatRooms
{
    public class ChatRoomFilterRequestModel
    {
        public string? Search { get; set; }
        public ChatFilterType type { get; set; } = ChatFilterType.All;
    }
}
