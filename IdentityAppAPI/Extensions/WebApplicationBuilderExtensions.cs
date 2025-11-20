
using API.Utility;
using IdentityAppAPI.Data;
using IdentityAppAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace IdentityAppAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            return builder;
        }

        public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder) {
            builder.Services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = SD.RequiredLength;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = SD.MaxFailedAccessAttemps;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(SD.DefaultLockoutTimeSpan);
            })
            .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();
            return builder;
        }
    }
}
