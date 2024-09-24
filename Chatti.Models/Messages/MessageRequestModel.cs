using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.Messages
{
    public class MessageRequestModel
    {
        public required string Content { get; set; }
        public required string ChatRoomId { get; set; }

    }

}