using Chatti.Services;

namespace Chatti.Api.Extensions
{
    public static class BusinessServicesConfigurationExtension
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<UsersService>();
            services.AddScoped<ClientsService>();

        }
    }
}
