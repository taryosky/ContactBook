using ContactBook.Models;

using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;

namespace ContactBook.Data
{
    public class DbInitializer
    {
        public static void Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ContactBookContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any()) return;
            if (context.Roles.Any()) return;

            User user = new User
            {
                FirstName = "Clement",
                LastName = "Azibataram",
                Email = "taryosky@gmail.com",
                PhoneNumber = "09088776543"
            };

            roleManager.CreateAsync(new IdentityRole(RolesEnum.Admin.ToString()));
            roleManager.CreateAsync(new IdentityRole(RolesEnum.Admin.ToString()));
            userManager.CreateAsync(user, "Azibataram@2452");
        }
    }
}
