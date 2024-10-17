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
        Task<ChatRoomResponseModel> CreateAsync(string userId, string tenantId, ChatRoomRequestModel model);
        Task<List<ChatRoomResponseModel>> ListByUserId(string UserId);
        Task<ChatRoomResponseModel> GetById(string chatroomId);
        Task DeleteAsync(string ChatRoomId);
        Task AddParticipantAsync(string adminId, string userId, string chatroomId);
        Task RemoveParticipantAsync(string adminId, string userId, string chatroomId);

        Task LeaveAsync(string userId, string chatroomId);

    }
}
