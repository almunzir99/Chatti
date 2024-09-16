using AutoMapper;
using Chatti.Entities;
using Chatti.Models.Users;

namespace Chatti.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, AuthenticationModel>().ReverseMap();


        }
    }
}
