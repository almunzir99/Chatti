﻿using Chatti.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Models.ChatRooms
{
    public class ChatRoomResponseModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<UserResponseModel> Participants { get; set; } = new();
    }
}