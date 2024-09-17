using Chatti.Core.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Entities
{
    public class User : EntityBase
    {
        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string? Username { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string? FullName { get; set; }
        public string? Email { get; set; }
        [Required]
        public byte[]? PasswordHash { get; set; }
        [Required]
        public byte[]? PasswordSalt { get; set; }
        public UserType Type { get; set; } = UserType.USER;
        public ObjectId ClientId { get; set; }

    }
}
