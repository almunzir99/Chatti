using Chatti.Models.ChatRooms;
using Chatti.Services.ChatRooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Authorize(Roles = "USER")]
    [Route("api/chat-rooms")]
    public class ChatRoomsController : BaseApiController
    {
        private readonly IChatRoomService chatRoomService;

        public ChatRoomsController(IChatRoomService chatRoomService)
        {
            this.chatRoomService = chatRoomService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateChatRoomAsync([FromBody] ChatRoomRequestModel model)
        {
            Request.Headers.TryGetValue("TenantId", out var tenantId);
            if (!model.Participants.Any())
                throw new Exception("You can't create empty chatroom");
            var result = await chatRoomService.CreateAsync(CurrentUserId, tenantId!, model);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetChatRoomsListAsync([FromQuery] ChatRoomFilterRequestModel filter)
        {
            var result = await chatRoomService.ListByUserId(CurrentUserId, filter);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatRoomById([FromRoute] string id)
        {

            var result = await chatRoomService.GetById(id);
            return Ok(result);
        }
        [HttpGet("{chatroomId}/participants/add/${userId}")]
        public async Task<IActionResult> AddParticipantChatRoomAsync([FromRoute] string chatroomId, string userId)
        {
            await chatRoomService.AddParticipantAsync(CurrentUserId, userId, chatroomId);
            return Ok(new
            {
                Message = "Participant added successfully"
            });
        }
        [HttpPost("{chatroomId}/participants/add")]
        public async Task<IActionResult> AddParticipantsChatRoomAsync([FromRoute] string chatroomId, [FromBody] List<string> particpantsIds)
        {
            await chatRoomService.AddParticipantsAsync(CurrentUserId, particpantsIds, chatroomId);
            return Ok(new
            {
                Message = "Participant added successfully"
            });
        }
        [HttpDelete("{chatroomId}/participants/remove/${userId}")]
        public async Task<IActionResult> DeleteParticipantChatRoomAsync([FromRoute] string chatroomId, string userId)
        {
            await chatRoomService.RemoveParticipantAsync(CurrentUserId, userId, chatroomId);
            return Ok(new
            {
                Message = "Participant removed successfully"
            });
        }
        [HttpDelete("{chatroomId}/participants/leave")]
        public async Task<IActionResult> LeaveChatRoomAsync([FromRoute] string chatroomId, string userId)
        {
            if (userId == CurrentUserId)
                return Forbid("You aren't allowed to do this operation");
            await chatRoomService.LeaveAsync(chatroomId, userId);
            return Ok(new
            {
                Message = "you left the chatroom successfully"
            });
        }
    }
}
