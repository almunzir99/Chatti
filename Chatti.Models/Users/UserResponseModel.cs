﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.Users
{
    public class UserResponseModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
