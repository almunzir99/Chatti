using AutoMapper;
using Chatti.Entities;
using Chatti.Models;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services
{
    public class ClientsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper mapper;

        public ClientsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<ClientResponseModel>> GetAllAsync()
        {
            var list = await _dbContext.Clients.ToListAsync();
            return mapper.Map<List<ClientResponseModel>>(list);
        }
        public async Task<ClientResponseModel> CreateAsync(ClientRequestModel model)
        {
            var entity = mapper.Map<Client>(model);
            await _dbContext.Clients.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return mapper.Map<ClientResponseModel>(entity);

        }
        public async Task<ClientResponseModel> UpdateAsync(string id, ClientRequestModel model)
        {
            var target = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (target == null)
            {
                throw new Exception("Invalid client id");
            }
            mapper.Map(model, target);
            _dbContext.Clients.Update(target);
            target.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return mapper.Map<ClientResponseModel>(target);

        }
        public async Task DeleteAsync(string id)
        {
            var target = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (target == null)
            {
                throw new Exception("Invalid client id");
            }
            target.Status = Core.Enums.StatusEnum.Deleted;
            _dbContext.Clients.Update(target);
            await _dbContext.SaveChangesAsync();
        }
    }
}
