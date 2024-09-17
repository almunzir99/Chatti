using AutoMapper;
using Chatti.Core.Helpers;
using Chatti.Entities;
using Chatti.Models.Users;
using Chatti.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<UserResponse> Authenticate(AuthenticationModel model)
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
            if (model.SystemId != null && !user.ClientId.Equals(model.SystemId))
                throw new Exception("invalid system id");
            var mappedUser = _mapper.Map<UserResponse>(user);
            mappedUser.Token = GenerateToken(user);
            return mappedUser;

        }
        public async Task<UserResponse> Register(UserRequest model)
        {
            var target = await _dbContext.Users.Where(x => x.Status == Core.Enums.StatusEnum.Active)
                .FirstOrDefaultAsync(x => x.Username == model.Username);
            if (target != null)
                throw new Exception("Username is already used");
            var client = await _dbContext.Clients.FirstOrDefaultAsync(x => x.Id.Equals(target.ClientId));
            if (client == null)
                throw new Exception("Invalid clientId");
            byte[] passwordSalt = [];
            byte[] passwordHash = [];
            HashingHelper.CreateHashPassword(model.Password, out passwordHash, out passwordSalt);
            var newUser = new User()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                ClientId = new MongoDB.Bson.ObjectId(model.ClientId),

            };
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserResponse>(model);
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Type == Core.Enums.UserType.USER ? "USER" : "ADMIN"),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
