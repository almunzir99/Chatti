using Chatti.Core.Helpers;
using Chatti.Entities;
using Chatti.Models.Messages;
using Chatti.Services.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var messages = await service.ListAsync(CurrentUserId, chatroomId);
            foreach (var item in messages)
            {
                if (item.Attachment != null)
                {
                    var filePath = Path.Combine(webHostEnvironment.WebRootPath, item.Attachment.AttachmentPath, item.Attachment.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        var fileInfo = new FileInfo(filePath);
                        item.Attachment.SizeInKB = (fileInfo.Length) / 1024;
                    }

                }
            }
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
        [HttpGet("{messageId}/info")]
        public async Task<IActionResult> GetMessageAsync([FromRoute] string messageId)
        {
            var result = await service.GetMessageInfoAsync(CurrentUserId, messageId);

            return Ok(result);
        }
        private async Task<MessageAttachmentModel> UploadAsync(IFormFile file, string senderId, MessageRequestModel model)
        {
            var relativePath = Path.Combine("uploads", senderId, "chat-rooms", model.ChatRoomId.ToString());
            var path = Path.Combine(webHostEnvironment.WebRootPath, relativePath);
            var mimeType = MIMETypeHelper.GetMimeType(Path.GetExtension(file.FileName));
            Directory.CreateDirectory(path);
            using (var stream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            string? thumnailPath = null;
            if (mimeType == Core.Enums.MimeType.IMAGE)
            {
                thumnailPath = ImageHelper.GenerateThumbnail(Path.Combine(path, file.FileName));
            }
            var messageAttachment = new MessageAttachmentModel()
            {
                AttachmentPath = relativePath,
                FileName = file.FileName,
                Type = mimeType.ToString(),
                Thumbnail = thumnailPath == null ? null : Path.GetFileName(thumnailPath),

            };
            return messageAttachment;


        }



    }
}
