using Microsoft.AspNetCore.Identity;
using Shared.DBModels;
using System;

internal class DatabaseSeeder
{

    //FF: Startup.cs => Configure => Add the following parameters to the Method Signature: 
    //                               UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager
    //FF: Startup.cs => Configure => Add after app.UseAuthentication():
    //                                 DatabaseSeeder.SeedData(userManager, roleManager);
    //FF: This is how ApplicationDbContext Class should look like
    //public class ApplicationDbContext  public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    //                                            ApplicationUserRole, IdentityUserLogin<string>,IdentityRoleClaim<string>, IdentityUserToken<string>>
    internal static void SeedData(UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
    {
        SeedRoles(roleManager);
        SeedUsers(userManager);
    }

    public static void SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (userManager.FindByNameAsync("felice").Result == null)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = "felice@gmail.com";
            user.Email = "felice@gmail.com";
            //user.FirstName = "Felice Ferri";
            //user.BirthDate = new DateTime(1981, 1, 1);

            IdentityResult result = userManager.CreateAsync(user, "felice").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }

    }

    public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
    {
        if (!roleManager.RoleExistsAsync("Regular").Result)
        {
            ApplicationRole role = new ApplicationRole();
            role.Name = "Regular";
            role.Description = "Can rate and leave a comment for a restaurant.";
            IdentityResult roleResult = roleManager.
            CreateAsync(role).Result;
        }


        if (!roleManager.RoleExistsAsync("Owner").Result)
        {
            ApplicationRole role = new ApplicationRole();
            role.Name = "Owner";
            role.Description = "Can create restaurants and reply comments about owned restaurants.";
            IdentityResult roleResult = roleManager.
            CreateAsync(role).Result;
        }

        if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            ApplicationRole role = new ApplicationRole();
            role.Name = "Admin";
            role.Description = "Can edit/delete all users, restaurants, comments, and reviews.";
            IdentityResult roleResult = roleManager.
            CreateAsync(role).Result;
        }
    }
}
