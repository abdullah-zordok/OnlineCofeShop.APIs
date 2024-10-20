using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task  SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user=new AppUser()
                { 
                DisplayName="Ahmed Nasr",
                Email="ahmed.ali@linkdev.com",
                UserName="ahmed.ali",
                PhoneNumber="01122334455"
                };
                await _userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}
