using System;

namespace Stationery.ViewModels.Admin;

public class AdminUserListItemViewModel
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }
    public IReadOnlyList<string> Roles { get; set; } = [];
    public bool IsLocked { get; set; }

}
