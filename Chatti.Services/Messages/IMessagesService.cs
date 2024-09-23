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
        Task<MessageResponseModel> SendAsync(MessageRequestModel model, MessageAttachmentModel? attachment = null);
        Task<IList<MessageResponseModel>> ListAsync(string ChatRoomId, string search = "");
        Task DeleteAsync(string senderId);
        Task<MessageResponseModel> EditAsync(string senderId, string messageId, MessageRequestModel model);


    }
}
