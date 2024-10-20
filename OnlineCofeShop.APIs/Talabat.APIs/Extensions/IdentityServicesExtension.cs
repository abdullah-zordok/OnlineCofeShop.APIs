using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
            }
         ).AddEntityFrameworkStores<AppIdentityDbContext>(); //this for user store which act as repository for user manager and contain create async method 
            services.AddAuthentication(options =>
            {
                //To define Name of Default Schema
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //To Put default Scheme (Bearer) on any endpoint is Authorized
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options => {
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateAudience = true,
                     ValidAudience = configuration["JWT:ValidAudience"],
                     ValidateIssuer = true,
                     ValidIssuer = configuration["JWT:ValidIssuer"],
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDays"]))

                 };
             }).AddJwtBearer("Bearer02", options =>
             {

             });
            return services;
        }

    }
}
