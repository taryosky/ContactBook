using ContactBook.Models;

using Microsoft.AspNetCore.Identity;

using System;

namespace ContactBook.Data
{
    public class DbInitializer
    {
        public static void SeedDB(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("taryosky@gmail.com").Result == null)
            {
                User user = new User();
                user.UserName = "taryosky@gmail.com";
                user.Email = "taryosky@gmail.com";
                user.FirstName = "Clement";
                user.LastName = "Azibataram";

                IdentityResult result = userManager.CreateAsync(user, "Azibataram@2452").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RolesEnum.Admin.ToString()).Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(RolesEnum.Regular.ToString()).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesEnum.Regular.ToString();
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(RolesEnum.Admin.ToString()).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = RolesEnum.Admin.ToString();
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
}
