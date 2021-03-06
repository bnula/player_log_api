﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public static class SeedData
    {
        public static async Task Seed(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync("administrator") == false)
            {
                var admin = new IdentityRole
                { 
                    Name = "administrator"
                };

                await roleManager.CreateAsync(admin);
            }

            if (await roleManager.RoleExistsAsync("user") == false)
            {
                var user = new IdentityRole
                {
                    Name = "user"
                };

                await roleManager.CreateAsync(user);
            }
        }

        private static async Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@page.com") == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin@page.com",
                    Email = "admin@page.com"
                };
                await userManager.CreateAsync(admin, "admin");
                await userManager.AddToRoleAsync(admin, "administrator");
            }

            if (await userManager.FindByEmailAsync("user@page.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "user1@page.com",
                    Email = "user1@page.com"
                };

                await userManager.CreateAsync(user, "123");
                await userManager.AddToRoleAsync(user, "user");

            }
        }
    }
}
