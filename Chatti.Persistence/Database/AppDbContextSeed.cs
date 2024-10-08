using Chatti.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatti.Persistence.Database
{
    public static class AppDbContextSeed
    {
        public async static Task SeedAsync(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                byte[] passwordSalt = [];
                byte[] passwordHash = [];
                HashingHelper.CreateHashPassword("@dm1n2012", out passwordHash, out passwordSalt);

                await context.Users.AddAsync(new Entities.Users.User()
                {
                    FullName = "Admin",
                    Username = "admin",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Type = Core.Enums.UserType.ADMIN,
                    Email = "admin@info.com"

                });
                await context.SaveChangesAsync();
            }

        }
    }
}
