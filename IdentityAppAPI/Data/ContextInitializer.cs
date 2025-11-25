using API.Utility;
using IdentityAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityAppAPI.Data
{
    public static class ContextInitializer
    {
        public static async Task InitializeAsync(Context context, UserManager<AppUser> userManager)
        {
            if(context.Database.GetPendingMigrations().Count() > 0)
            {
                await context.Database.MigrateAsync();
            }

            if(!userManager.Users.Any())
            {
                var john = new AppUser
                {
                    Name = "JOHN",
                    UserName = "john",
                    Email = "john@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                };
                await userManager.CreateAsync(john, SD.DefaultPassword);

                var peter = new AppUser
                {
                    Name = "PETER",
                    UserName = "peter",
                    Email = "peter@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                };
                await userManager.CreateAsync(peter, SD.DefaultPassword);

                var tom = new AppUser
                {
                    Name = "TOM",
                    UserName = "tom",
                    Email = "tom@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                };
                await userManager.CreateAsync(tom, SD.DefaultPassword);

                var bob = new AppUser
                {
                    Name = "BOB",
                    UserName = "bob",
                    Email = "bob@example.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                };
                await userManager.CreateAsync(bob, SD.DefaultPassword);
            }
        }
    }
}
