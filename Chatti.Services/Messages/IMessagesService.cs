using Chatti.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services.Messages
{
    public interface IMessagesService
    {
        Task<MessageResponseModel> SendAsync(MessageRequestModel model, string senderId, MessageAttachmentModel? attachment = null);
        Task<IList<MessageResponseModel>> ListAsync(string userId, string ChatRoomId, string search = "");
        Task DeleteAsync(string senderId, string messageId);
        Task<MessageResponseModel> EditAsync(string senderId, string messageId, MessageRequestModel model);
        // get message info
        Task<MessageResponseModel> GetMessageInfoAsync(string senderId, string messageId);


    }
}
