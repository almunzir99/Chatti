using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class Client : EntityBase
    {
        public required string Name { get; set; }
    }
}
