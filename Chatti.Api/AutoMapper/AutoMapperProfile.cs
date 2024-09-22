using AutoMapper;
using Chatti.Entities;
using Chatti.Models;
using Chatti.Models.ChatRooms;
using Chatti.Models.Users;

namespace Chatti.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseModel>().ForMember(c => c.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();
            CreateMap<User, AuthenticationModel>().ReverseMap();

            CreateMap<Client, ClientResponseModel>().ForMember(c => c.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();
            CreateMap<Client, ClientRequestModel>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomResponseModel>().ReverseMap();

        }
    }
}
