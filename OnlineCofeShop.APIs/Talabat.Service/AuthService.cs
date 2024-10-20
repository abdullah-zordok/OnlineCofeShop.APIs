using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration) {
           _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            //Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)

            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role,role));
            }
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])); //put SecretKey at appsettings

            //token: token object use it to build token
            var token = new JwtSecurityToken(
                ///claims : 
                ///Registered claims: These are a set of predefined claims which are not mandatory but recommended,
                ///to provide a set of useful, interoperable claims. Some of them are: iss (issuer),
                ///exp (expiration time), sub (subject), aud (audience)
                ///OR
                ///Private claims: These are the custom claims created to share information between parties

                //1.Put Registered claims
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                //2.Put Private claims
                claims: authClaims,

                //3.Send Secret key and security Algo
                signingCredentials: new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature)

                );

            //return Token iteself
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
