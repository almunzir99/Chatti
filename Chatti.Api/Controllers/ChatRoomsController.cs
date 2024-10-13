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
        [HttpGet()]
        public async Task<IActionResult> GetChatRoomsListAsync()
        {
            var result = await chatRoomService.ListByUserId(CurrentUserId);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatRoomById([FromRoute] string id)
        {

            var result = await chatRoomService.GetById(id, CurrentUserId);
            return Ok(result);
        }
    }
}
