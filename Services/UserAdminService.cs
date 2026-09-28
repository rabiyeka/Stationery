using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Stationery.Models;
using Stationery.ViewModels.Admin;

namespace Stationery.Services;

public class UserAdminService : IUserAdminService
{
    private static readonly string[] AllowedRoles = ["Admin","Customer"];
    private readonly UserManager<StationeryUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserAdminService(UserManager<StationeryUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<(bool Success, string? Error)> CreateUserAsync(AdminUserCreateViewModel model)
    {
        if (!AllowedRoles.Contains(model.Role))
        {
            return (false, "Geçersiz rol.");
        }
        var user = new StationeryUser
        {
            UserName=model.Email.Trim(),
            Email=model.Email.Trim(),
            FullName=model.FullName?.Trim(),
            EmailConfirmed=true
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return (false, string.Join(" ", result.Errors.Select(e=>e.Description)));
        }
        await _userManager.AddToRoleAsync(user, model.Role);
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteUserAsync(string id, string currentUserId)
    {
        if (id == currentUserId)
        {
            return (false, "Kendi hesabınızı silemezsiniz.");
        }
        var user = await _userManager.FindByIdAsync(id);
        if(user is null) return (false, "Kullanıcı bulunamadı");
        if(await _userManager.IsInRoleAsync(user, "Admin") && await GetAdminCountAsync() <= 1)
        {
            return (false, "Son admin kullanıcısı silinemez.");
        }
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return (false, string.Join(" ", result.Errors.Select(e => e.Description)));
        }
        return (true, null);
    }

    private async Task<int> GetAdminCountAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        return admins.Count;
    }


    public async Task<AdminUserFormViewModel?> GetUserForEditAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user is null) return null;
        var roles = await _userManager.GetRolesAsync(user);
        return new AdminUserFormViewModel
        {
            Id= user.Id,
            Email = user.Email ?? user.UserName ?? string.Empty,
            FullName=user.FullName,
            Role = roles.FirstOrDefault(r=>AllowedRoles.Contains(r)) ?? "Customer", IsLocked = IsUserLocked(user)
        };
    }

    private static bool IsUserLocked(StationeryUser user) => user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd>DateTimeOffset.UtcNow;


    public async Task<IList<AdminUserListItemViewModel>> GetUsersForAdminAsync()
    {
        var users = await _userManager.Users.OrderBy(u=>u.Email).ToListAsync();
        var result = new List<AdminUserListItemViewModel>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new AdminUserListItemViewModel
            {
                Id=user.Id,
                Email= user.Email ?? user.UserName ?? string.Empty,
                FullName=user.FullName,
                Roles= roles.ToList(),
                IsLocked=IsUserLocked(user)
            });
            
        }
        return result;
    }

    public async Task<(bool Success, string? Error)> SetLockoutAsync(string id, bool lockUser, string currentUserId)
    {
        if(id==currentUserId && lockUser)
        {
            return (false, "Kendi hesabınızı kilitleyemezsiniz.");
        }
        var user = await _userManager.FindByIdAsync(id);
        if(user is null) return (false, "Kullanıcı bulunamadı.");
        if (lockUser)
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> UpdateUserAsync(string id, AdminUserFormViewModel model, string currentUserId)
    {
        if(!AllowedRoles.Contains(model.Role))
        {
            return (false, "Geçersiz rol");
        };
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return (false, "Kullanıcı bulunamadı.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        var wasAdmin = currentRoles.Contains("Admin");
        var willBeAdmin = model.Role == "Admin";
        if(wasAdmin && !willBeAdmin)
        {
            if(id==currentUserId)
            {
                return (false, "Kendi admin rolünüzü değiştiremezsiniz.");
            }
            if(await GetAdminCountAsync() <= 1)
            {
                return (false, "Son admin kullanıcısının rolünü değiştiremezsiniz.");
            }
        }
        var email = model.Email.Trim();
        if(!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if(existing is not null && existing.Id != user.Id)
            {
                return (false, "Bu email başka bir kullanıcı tarafından kullanılıyor.");
            }
            user.Email = email;
            user.UserName=email;
        }
        user.FullName=model.FullName?.Trim();
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return (false, string.Join(" ", updateResult.Errors.Select(e=>e.Description)));
        }

        foreach (var role in currentRoles.Where(r => AllowedRoles.Contains(r)))
        {
            await _userManager.RemoveFromRoleAsync(user, role);
        }
        await _userManager.AddToRoleAsync(user, model.Role);
        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user); // şifre değiştirme token
            var passwordResult = await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
            if (!passwordResult.Succeeded)
            {
                return (false, string.Join(" ", passwordResult.Errors.Select(e => e.Description)));
            }
        }
        return (true, null);

    }
}
