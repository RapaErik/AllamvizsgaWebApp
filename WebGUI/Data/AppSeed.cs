using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGUI.Models;

namespace WebGUI.Data
{
    public class AppSeed
    {
        public static async Task SeedAsync(IServiceProvider SvcProv)
        {
            var adminEmail = "admin@admin.com";
            var password = "1werwerwer";
            var UserManager = SvcProv.GetRequiredService<UserManager<ApplicationUser>>();
            var RoleManager = SvcProv.GetRequiredService<RoleManager<IdentityRole>>();
            string[] RoleNames = { "Admin", "Client" };
            IdentityResult RoleResult;

            foreach (var RoleName in RoleNames)
            {
                var RoleExists = await RoleManager.RoleExistsAsync(RoleName);

                if (!RoleExists)
                {
                    RoleResult = await RoleManager.CreateAsync(new IdentityRole(RoleName));
                }
            }
            //add admin
            if (UserManager.FindByEmailAsync(adminEmail).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = adminEmail,
                    NormalizedEmail = adminEmail,
                    EmailConfirmed = true,
                    UserName = adminEmail,
                    NormalizedUserName = adminEmail,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false
                };

                IdentityResult result = UserManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    UserManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
           
        }
    }
}
