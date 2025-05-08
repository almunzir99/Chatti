using AutoMapper;
using Chatti.Entities.ChatRooms;
using Chatti.Entities.Users;
using Chatti.Models.ChatRooms;
using Chatti.Models.Messages;
using Chatti.Models.Users;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services.ChatRooms
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper _mapper;

        public ChatRoomService(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ChatRoomDetailReponseModel> CreateAsync(string adminId, string tenantId, ChatRoomRequestModel model)
        {
            if (!model.Participants.Any(x => x == adminId))
                model.Participants.Add(adminId);
            var entity = new ChatRoom()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Status = Core.Enums.StatusEnum.Active,
                TenantId = new MongoDB.Bson.ObjectId(tenantId),
                Settings = new ChatRoomSettings()
                {
                    ReceiveNotifications = true,
                }

            };
            await dbContext.ChatRooms.AddAsync(entity);
            entity.Participants = model.Participants.Select(x => new ChatRoomParticipant()
            {
                UserId = new MongoDB.Bson.ObjectId(x),
                IsAdmin = adminId.Equals(x),

            }).ToList();
            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();
            var loadedParticipants = await dbContext.Users.Where(x => model.Participants.Contains(x.Id.ToString())).ToListAsync();
            var result = new ChatRoomDetailReponseModel()
            {
                Id = entity.Id.ToString(),
                Name = model.Name,
                Participants = _mapper.Map<List<UserResponseModel>>(loadedParticipants)

            };
            return result;

        }

        public Task DeleteAsync(string ChatRoomId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChatRoomResponseModel>> ListByUserId(string UserId, ChatRoomFilterRequestModel filter)
        {
            // fetch chatrooms
            var query = dbContext.ChatRooms
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => filter.Search == null || x.Name.Contains(filter.Search))
                .Where(x => x.Participants.Any(x => x.UserId.ToString().Equals(UserId)))      .AsQueryable();
            // apply filters
            switch (filter.type)
            {
                case Core.Enums.ChatFilterType.All:
                    break;
                case Core.Enums.ChatFilterType.Single:
                    query = query.Where(x => x.Participants.Count == 2);
                    break;
                case Core.Enums.ChatFilterType.Group:
                    query = query.Where(x => x.Participants.Count > 2);
                    break;
                case Core.Enums.ChatFilterType.Favorite:
                    break;
                case Core.Enums.ChatFilterType.Archive:
                    break;
                default:
                    break;
            }


            var chatrooms = await query.ToListAsync();
            var chatroomIds = query.Select(x => x.Id).ToList();
            var lastMessages = (await dbContext.Messages
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => chatroomIds.Any(c => c.Equals(x.ChatRoomId))).ToListAsync())
                .GroupBy(x => x.ChatRoomId).Select(x => new
                {
                    x.First().ChatRoomId,
                    Message = x.OrderByDescending(x => x.CreatedOn).First(),
                    UnreadMessagesCount = x.Where(x => !x.SeenBy.Any(x => x.UserId.ToString().Equals(UserId))).Count()
                }).ToList();


            var result = chatrooms.Select(x =>
            {
                var message = lastMessages.FirstOrDefault(m => m.ChatRoomId.Equals(x.Id));
                return new ChatRoomResponseModel()
                {
                    Name = x.Name,
                    Id = x.Id.ToString(),
                    LastMessage = new ChatRoomLastMessageResponseModel()
                    {
                        Content = message == null ? String.Empty : string.IsNullOrEmpty(message!.Message.Content) && message!.Message.Attachment != null ? "send you an attachment" : message!.Message.Content,
                        Sender = message?.Message.Sender.FullName ?? string.Empty,
                        SentAt = message?.Message.CreatedOn ?? DateTime.Now,
                        Id = string.Empty

                    },
                    UnreadMessagesCount = message?.UnreadMessagesCount ?? 0

                };
            }).ToList();
            return result;
        }
        public async Task<ChatRoomDetailReponseModel> GetById(string chatroomId)
        {
            // get chat room
            var chatroom = await dbContext.ChatRooms.Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .FirstOrDefaultAsync(x => x.Id.ToString().Equals(chatroomId));
            if (chatroom == null)
            {
                throw new Exception("Invalid chatroom id");
            }
            // get participant users response
            var users = await dbContext.Users
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(user => chatroom.Participants.Any(x => x.UserId.Equals(user.Id))).ToListAsync();

            // get attachments
            var messages = await dbContext.Messages
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => x.Attachment != null)
                .Where(x => x.ChatRoomId.ToString().Equals(chatroomId))
                .AsNoTracking()
                .ToListAsync();

            // get 
            return new ChatRoomDetailReponseModel()
            {
                Id = chatroomId,
                Name = chatroom!.Name,
                Participants = _mapper.Map<List<UserResponseModel>>(users),
                Attachments = _mapper.Map<List<MessageAttachmentModel>>(messages.Select(x => x.Attachment).ToList()),
                Settings = _mapper.Map<ChatRoomSettingsResponseModel>(chatroom.Settings)
            };
        }

        public async Task AddParticipantAsync(string adminId, string userId, string chatroomId)
        {
            var chatroom = await dbContext.ChatRooms
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => x.Id.ToString().Equals(chatroomId))
                .Where(x => x.Participants.Any(x => x.IsAdmin && x.UserId.ToString().Equals(adminId)))
                .FirstOrDefaultAsync();
            if (chatroom == null) { throw new Exception("Invalid chatroom id"); }
            if (chatroom.Participants.Any(x => x.UserId.ToString().Equals(userId)))
            {
                throw new Exception("The new user is already participant in chatroom ");
            }
            chatroom.Participants.Add(new ChatRoomParticipant()
            {
                IsAdmin = false,
                UserId = new MongoDB.Bson.ObjectId(userId)
            });
            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveParticipantAsync(string adminId, string userId, string chatroomId)
        {
            var chatroom = await dbContext.ChatRooms
              .Where(x => x.Status == Core.Enums.StatusEnum.Active)
              .Where(x => x.Id.ToString().Equals(chatroomId))
              .Where(x => x.Participants.Any(x => x.IsAdmin && x.UserId.ToString().Equals(adminId)))
              .FirstOrDefaultAsync();
            if (chatroom == null) { throw new Exception("Invalid chatroom id"); }

            var target = chatroom.Participants.FirstOrDefault(x => x.UserId.ToString().Equals(userId));
            if (target == null)
            {
                throw new Exception("The target participant isn't available");
            }
            chatroom.Participants.Remove(target);
            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();
        }

        public async Task LeaveAsync(string userId, string chatroomId)
        {
            var chatroom = await dbContext.ChatRooms
               .Where(x => x.Status == Core.Enums.StatusEnum.Active)
               .Where(x => x.Id.ToString().Equals(chatroomId))
               .FirstOrDefaultAsync();
            if (chatroom == null) { throw new Exception("Invalid chatroom id"); }

            var target = chatroom.Participants.FirstOrDefault(x => x.UserId.ToString().Equals(userId));
            if (target == null)
            {
                throw new Exception("The target participant isn't available");
            }
            chatroom.Participants.Remove(target);
            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();
        }
    }
}
