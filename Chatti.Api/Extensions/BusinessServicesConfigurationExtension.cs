using Chatti.Services;
using Chatti.Services.ChatRooms;
using Chatti.Services.Messages;

namespace Chatti.Api.Extensions
{
    public static class BusinessServicesConfigurationExtension
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<UsersService>();
            services.AddScoped<TenantsService>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<IMessagesService, MessagesService>();



        }
    }
}
