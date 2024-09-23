using AutoMapper;
using Chatti.Core.Enums;
using Chatti.Entities;
using Chatti.Models;
using Chatti.Models.ChatRooms;
using Chatti.Models.Messages;
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
            CreateMap<Message, MessageRequestModel>().ReverseMap();
            CreateMap<User, MessageSender>().ReverseMap();
            CreateMap<MessageSender, UserResponseModel>().ReverseMap();
            CreateMap<Message, MessageResponseModel>().ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();
            CreateMap<MessageAttachment, MessageAttachmentModel>().ForMember(x => x.Type, opt => opt.MapFrom(x => x.Type.ToString())).ReverseMap()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => (MimeType)Enum.Parse(typeof(MimeType), x.Type)));
            ;


        }
    }
}
