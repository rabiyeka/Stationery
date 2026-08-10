using System;
using Microsoft.AspNetCore.Identity;

namespace Stationery.Models;

public class StationeryUser : IdentityUser
{
    public string? FullName { get; set;}
    public string? Address { get; set;}
}
