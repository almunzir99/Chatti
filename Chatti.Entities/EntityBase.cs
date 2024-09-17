using Chatti.Core.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class EntityBase
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public StatusEnum Status { get; set; } = StatusEnum.Active;
    }
}
