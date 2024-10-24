using AutoMapper;
using Chatti.Core.Enums;
using Chatti.Entities.ChatRooms;
using Chatti.Entities.Messages;
using Chatti.Entities.Tenants;
using Chatti.Entities.Users;
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
            CreateMap<Tenant, TenantResponseModel>().ForMember(c => c.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();
            CreateMap<Tenant, TenantRequestModel>().ReverseMap();
            CreateMap<ChatRoom, ChatRoomResponseModel>().ReverseMap();
            CreateMap<Message, MessageRequestModel>().ReverseMap();
            CreateMap<Message, MessageResponseModel>()
                .ForMember(x => x.SentAt, opt => opt.MapFrom(x => x.CreatedOn))
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id.ToString())).ReverseMap();

            CreateMap<User, MessageSender>().ReverseMap();
            CreateMap<MessageSender, MessageSenderModel>().ReverseMap();
            CreateMap<MessageAttachment, MessageAttachmentModel>().ForMember(x => x.Type, opt => opt.MapFrom(x => x.Type.ToString())).ReverseMap()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => (MimeType)Enum.Parse(typeof(MimeType), x.Type)));
            CreateMap<MessageSeenBy, MessageSeenByResponse>().ReverseMap();
            CreateMap<ChatRoomSettings, ChatRoomSettingsResponseModel>().ReverseMap();


        }
    }
}
