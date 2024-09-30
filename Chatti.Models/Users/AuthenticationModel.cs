using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.Users
{
    public class AuthenticationModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? TenantId { get; set; }
    }
}
