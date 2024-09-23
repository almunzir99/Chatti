using AutoMapper;
using Chatti.Entities;
using Chatti.Models.ChatRooms;
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

        public async Task<ChatRoomResponseModel> CreateAsync(ChatRoomRequestModel model)
        {
            if (!model.Participants.Any(x => x == model.AdminId))
                model.Participants.Add(model.AdminId);
            var entity = new ChatRoom()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                Status = Core.Enums.StatusEnum.Active,

            };
            await dbContext.ChatRooms.AddAsync(entity);
            entity.Participants = model.Participants.Select(x => new ChatRoomParticipant()
            {
                UserId = new MongoDB.Bson.ObjectId(x),
                IsAdmin = model.AdminId.Equals(x),

            }).ToList();
            dbContext.ChangeTracker.DetectChanges();
            await dbContext.SaveChangesAsync();
            var loadedParticipants = await dbContext.Users.Where(x => model.Participants.Contains(x.Id.ToString())).ToListAsync();
            var result = new ChatRoomResponseModel()
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

        public async Task<List<ChatRoomResponseModel>> ListByUserId(string UserId)
        {
            // fetch chatrooms
            var chatrooms = await dbContext.ChatRooms
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => x.Participants.Any(x => x.UserId.ToString().Equals(UserId))).ToListAsync();
            // fetch users
            var users = await dbContext.Users.Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => chatrooms.SelectMany(x => x.Participants).Any(d => d.UserId.Equals(x.Id))).ToListAsync();
            // distribute each user for each chat room 
            var result = chatrooms.Select(x =>
            { 
                List<User> chatroomUsers = users.Where(u => x.Participants.Any(p => p.UserId.Equals(u.Id))).ToList();
                return new ChatRoomResponseModel()
                {
                    Participants = _mapper.Map<List<UserResponseModel>>(chatroomUsers),
                    Name = x.Name,
                    Id = x.Id.ToString(),
                };
            }).ToList();
            return result;
        }
    }
}
