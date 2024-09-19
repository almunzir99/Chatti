using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class ChatRoom : EntityBase
    {
        public required string Name { get; set; }
        public IList<User> UserIds { get; set; } = new List<User>();
        public required User CreatedBy { get; set; }
    }
}
