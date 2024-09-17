using Chatti.Persistence.Database;

namespace Chatti.Api.Extensions
{
    public static class DbContextSeedExtension
    {
        public static async Task DbContextSeedAsync(this WebApplication host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();

                await AppDbContextSeed.SeedAsync(context);
            }

            await host.RunAsync();
        }
    }
}
