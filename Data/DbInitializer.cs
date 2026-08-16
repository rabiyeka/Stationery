using System;
using Microsoft.AspNetCore.Identity;
using Stationery.Models;

namespace Stationery.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<StationeryUser>>();

        string[] roles = { "Admin", "Customer" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        await SeedUserAsync(userManager, "rabiye@admin.com", "Admin123.", "Rabiye Kargin", "Admin");
    }
    private static async Task SeedUserAsync(UserManager<StationeryUser>userManager, string email, string password, string fullName,string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null) return;
        user = new StationeryUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }

}
