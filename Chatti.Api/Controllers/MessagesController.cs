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
            model.SenderId = CurrentUserId;
            var messageAttachment = attachment != null ? await UploadAsync(attachment, model) : null;
            var result = await service.SendAsync(model, messageAttachment);
            return Ok(result);
        }
        [HttpGet("{chatroomId}")]
        public async Task<IActionResult> GetMessagesAsync([FromRoute] string chatroomId)
        {
            var messages = await service.ListAsync(chatroomId);
            return Ok(messages);
        }
        private async Task<MessageAttachmentModel> UploadAsync(IFormFile file, MessageRequestModel model)
        {
            var path = Path.Combine(webHostEnvironment.WebRootPath, "uploads", model.SenderId!.ToString(), "chat-rooms", model.ChatRoomId.ToString());
            Directory.CreateDirectory(path);
            using (var stream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var messageAttachment = new MessageAttachmentModel()
            {
                AttachmentPath = path,
                FileName = file.FileName,
                Type = MIMETypeHelper.GetMimeType(Path.GetExtension(file.FileName)).ToString(),

            };
            return messageAttachment;


        }



    }
}
