using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class MessageSender
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public string? FullName { get; set; }

    }
}
