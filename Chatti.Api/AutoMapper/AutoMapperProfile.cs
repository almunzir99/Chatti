using AutoMapper;
using Chatti.Entities;
using Chatti.Models.Users;

namespace Chatti.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseModel>().ReverseMap();
            CreateMap<User, UserRequestModel>().ReverseMap();


        }
    }
}
