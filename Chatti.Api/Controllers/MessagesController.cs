using Chatti.Core.Helpers;
using Chatti.Entities;
using Chatti.Models.Messages;
using Chatti.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chatti.Api.Controllers
{
    [Authorize(Roles = "USER")]
    [Route("api/messages")]
    public class MessagesController : BaseApiController
    {
        private readonly IMessagesService service;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MessagesController(IMessagesService service, IWebHostEnvironment webHostEnvironment)
        {
            this.service = service;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendAsync(IFormFile? attachment, [FromForm] MessageRequestModel model)
        {
            var messageAttachment = attachment != null ? await UploadAsync(attachment, CurrentUserId, model) : null;
            var result = await service.SendAsync(model, CurrentUserId, messageAttachment);
            return Ok(result);
        }
        [HttpGet("{chatroomId}")]
        public async Task<IActionResult> GetMessagesAsync([FromRoute] string chatroomId)
        {
            var messages = await service.ListAsync(chatroomId);
            return Ok(messages);
        }
        [HttpPut("{messageId}")]
        public async Task<IActionResult> EditMessageAsync([FromRoute] string messageId, MessageRequestModel model)
        {
            var result = await service.EditAsync(CurrentUserId, messageId, model);
            return Ok(result);

        }
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessageAsync([FromRoute] string messageId)
        {
            await service.DeleteAsync(CurrentUserId, messageId);
            return Ok("Message deleted successfully");

        }
        private async Task<MessageAttachmentModel> UploadAsync(IFormFile file, string senderId, MessageRequestModel model)
        {
            var relativePath = Path.Combine("uploads", senderId, "chat-rooms", model.ChatRoomId.ToString());
            var path = Path.Combine(webHostEnvironment.WebRootPath, relativePath);
            Directory.CreateDirectory(path);
            using (var stream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var messageAttachment = new MessageAttachmentModel()
            {
                AttachmentPath = relativePath,
                FileName = file.FileName,
                Type = MIMETypeHelper.GetMimeType(Path.GetExtension(file.FileName)).ToString(),

            };
            return messageAttachment;


        }



    }
}
