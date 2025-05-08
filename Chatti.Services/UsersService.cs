using AutoMapper;
using Chatti.Core.Helpers;
using Chatti.Entities.Users;
using Chatti.Models.Users;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chatti.Services
{
    public class UsersService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public UsersService(IConfiguration configuration, AppDbContext dbContext, IMapper mapper)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserResponseModel> Authenticate(string? tenantId, AuthenticationModel model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == model.UserName);
            if (user == null)
            {
                throw new Exception("Invalid username");
            }
            // validate password
            var result = HashingHelper.VerifyPassword(model.Password, user.PasswordHash!, user.PasswordSalt!);
            if (!result)
                throw new Exception("invalid password");
            if (user.Type != Core.Enums.UserType.Admin && (tenantId == null || !user.TenantId.ToString().Equals(tenantId)))
                throw new Exception("invalid Tenant id");
            var mappedUser = _mapper.Map<UserResponseModel>(user);
            mappedUser.Token = GenerateToken(user);
            return mappedUser;

        }
        public async Task<UserResponseModel> Register(string? tenantId, UserRequestModel model)
        {
            var target = await _dbContext.Users.Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .FirstOrDefaultAsync(x => x.Username == model.Username);
            if (target != null)
                throw new Exception("Username is already used");
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id.ToString().Equals(tenantId));
            if (tenant == null)
                throw new Exception("Invalid tenantId");

            HashingHelper.CreateHashPassword(model.Password, out var passwordHash, out var passwordSalt);
            var newUser = new User()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                TenantId = new MongoDB.Bson.ObjectId(tenantId),
                Type = Core.Enums.UserType.User,


            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            UserResponseModel userResponseModel = _mapper.Map<UserResponseModel>(newUser);
            userResponseModel.Token = GenerateToken(newUser);
            return userResponseModel;
        }


        public async Task ResetPassword(string userId, string newPassword)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString().Equals(userId));
            if (user == null) throw new Exception("Invalid user id");
            HashingHelper.CreateHashPassword(newPassword, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString().Equals(userId));
            if (user == null) throw new Exception("Invalid user id");
            user.Status = Core.Enums.StatusEnum.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Type == Core.Enums.UserType.User ? "USER" : "ADMIN"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public async Task<List<UserResponseModel>> GetUsersListAsync()
        {
            var users = await _dbContext.Users
                .Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .Where(x => x.Type == Core.Enums.UserType.User)
                .ToListAsync();
            return _mapper.Map<List<UserResponseModel>>(users);
        }

        public async Task<List<UserResponseModel>> GetContactsAsync(string tenantId, string userId)
        {
            var users = await _dbContext.Users
               .Where(x => x.Status == Core.Enums.StatusEnum.Active)
               .Where(x => x.Type == Core.Enums.UserType.User)
               .Where(x => x.TenantId.ToString().Equals(tenantId))
               .Where(x => !x.Id.ToString().Equals(userId))
               .ToListAsync();
            return _mapper.Map<List<UserResponseModel>>(users);

        }
    }
}
