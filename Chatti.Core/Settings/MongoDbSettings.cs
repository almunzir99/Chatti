using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Core.Settings
{
    public class MongoDbSettings
    {
        public required string AtlasURI { get; set; }
        public required string DatabaseName { get; set; } 
    }
}
