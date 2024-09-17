using AutoMapper;
using Chatti.Entities;
using Chatti.Models.Users;

namespace Chatti.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponse>().ForMember(c => c.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();
            CreateMap<User, AuthenticationModel>().ReverseMap();


        }
    }
}
