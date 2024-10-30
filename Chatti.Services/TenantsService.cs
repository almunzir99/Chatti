using AutoMapper;
using Chatti.Entities.Tenants;
using Chatti.Models.Tenants;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Services
{
    public class TenantsService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper mapper;

        public TenantsService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<TenantResponseModel>> GetAllAsync()
        {
            var list = await _dbContext.Tenants.ToListAsync();
            return mapper.Map<List<TenantResponseModel>>(list);
        }
        public async Task<TenantResponseModel> CreateAsync(TenantRequestModel model)
        {
            var entity = mapper.Map<Tenant>(model);
            await _dbContext.Tenants.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return mapper.Map<TenantResponseModel>(entity);

        }
        public async Task<TenantResponseModel> UpdateAsync(string id, TenantRequestModel model)
        {
            var target = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (target == null)
            {
                throw new Exception("Invalid tenant id");
            }
            mapper.Map(model, target);
            _dbContext.Tenants.Update(target);
            target.ModifiedOn = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return mapper.Map<TenantResponseModel>(target);

        }
        public async Task DeleteAsync(string id)
        {
            var target = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (target == null)
            {
                throw new Exception("Invalid tenant" +
                    " id");
            }
            target.Status = Core.Enums.StatusEnum.Deleted;
            _dbContext.Tenants.Update(target);
            await _dbContext.SaveChangesAsync();
        }
    }
}
