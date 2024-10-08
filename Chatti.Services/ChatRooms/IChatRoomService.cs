using Chatti.Models.ChatRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services.ChatRooms
{
    public interface IChatRoomService
    {
        Task<ChatRoomResponseModel> CreateAsync(ChatRoomRequestModel model);
        Task<List<ChatRoomResponseModel>> ListByUserId(string UserId);
        Task DeleteAsync(string ChatRoomId);
        Task SeeMessagesAsync(string chatroomId, string userId);
    }
}
