using System;
using Microsoft.AspNetCore.Identity;

namespace EcommerceAPI.Models;

public class ApplicationUser: IdentityUser
{
    public string? Name { get; set; }
}
