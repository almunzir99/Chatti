using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities.Tenants
{
    public class Tenant : EntityBase
    {
        public required string TenantName { get; set; }
    }
}
