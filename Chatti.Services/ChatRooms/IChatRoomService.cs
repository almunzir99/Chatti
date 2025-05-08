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
        Task<ChatRoomDetailReponseModel> CreateAsync(string userId, string tenantId, ChatRoomRequestModel model);
        Task<List<ChatRoomResponseModel>> ListByUserId(string UserId, ChatRoomFilterRequestModel filter);
        Task<ChatRoomDetailReponseModel> GetById(string chatroomId);
        Task DeleteAsync(string ChatRoomId);
        Task AddParticipantAsync(string adminId, string userId, string chatroomId);
        Task RemoveParticipantAsync(string adminId, string userId, string chatroomId);

        Task LeaveAsync(string userId, string chatroomId);

    }
}
