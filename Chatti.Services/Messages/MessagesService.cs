﻿using AutoMapper;
using Chatti.Entities;
using Chatti.Models.Messages;
using Chatti.Models.Users;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services.Messages
{
    public class MessagesService : IMessagesService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;

        public MessagesService(AppDbContext appContext, IMapper mapper)
        {
            this.dbContext = appContext;
            this.mapper = mapper;
        }

        public async Task<IList<MessageResponseModel>> ListAsync(string ChatRoomId, string search = "")
        {
            var messages = await dbContext.Messages.Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => x.ChatRoomId.ToString().Equals(ChatRoomId))
                .Where(x => x.Content.Contains(search, StringComparison.CurrentCultureIgnoreCase))
                .ToListAsync();

            return mapper.Map<IList<MessageResponseModel>>(messages);

        }

        public async Task<MessageResponseModel> SendAsync(MessageRequestModel model, string senderId, MessageAttachmentModel? attachment = null)
        {
            var message = mapper.Map<Message>(model);
            message.Attachment = attachment == null ? null : mapper.Map<MessageAttachment>(attachment);
            var sender = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString().Equals(senderId));
            if (sender == null)
                throw new Exception("Invalid sender id");
            var chatroom = await dbContext.ChatRooms.FirstOrDefaultAsync(x => x.Id.ToString().Equals(model.ChatRoomId) && x.Participants.Any(p => p.UserId.ToString().Equals(senderId)));
            if (chatroom == null)
                throw new Exception("Invalid chatroom id");

            message.Sender = mapper.Map<MessageSender>(sender);
            message.Sender.UserId = senderId;
            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();
            return mapper.Map<MessageResponseModel>(message);
        }
        public async Task<MessageResponseModel> EditAsync(string senderId, string messageId, MessageRequestModel model)
        {
            var message = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id.ToString().Equals(messageId) && x.Sender.UserId.Equals(senderId));
            if (message == null) throw new Exception("Message not found");
            message.Content = model.Content;
            message.ModifiedOn = DateTime.Now;
            await dbContext.SaveChangesAsync();
            return mapper.Map<MessageResponseModel>(message);
        }
        public async Task DeleteAsync(string senderId, string messageId)
        {
            var message = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id.ToString().Equals(messageId) && x.Sender.UserId.Equals(senderId));
            if (message == null) throw new Exception("Message not found");
            message.Status = Core.Enums.StatusEnum.Deleted;
            await dbContext.SaveChangesAsync();
        }
    }
}
