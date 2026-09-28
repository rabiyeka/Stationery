using System;
using Stationery.ViewModels.Admin;

namespace Stationery.Services;

public interface IUserAdminService
{
    Task<IList<AdminUserListItemViewModel>> GetUsersForAdminAsync();
    Task<AdminUserFormViewModel?> GetUserForEditAsync(string id);
    Task<(bool Success, string? Error)> CreateUserAsync(AdminUserCreateViewModel model);
    Task<(bool Success, string? Error)> UpdateUserAsync(string id, AdminUserFormViewModel model, string currentUserId);
    Task<(bool Success, string? Error)> SetLockoutAsync(string id, bool lockUser, string currentUserId);
    Task<(bool Success, string? Error)> DeleteUserAsync(string id, string currentUserId);

}
