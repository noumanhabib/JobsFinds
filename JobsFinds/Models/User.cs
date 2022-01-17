using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobsFinds.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = "";

    public UserType Type { get; set; }

    public string? image { get; set; } = "users/default_profile.png";
}

public enum UserType
{
    Admin,
    Student,
    Company
}
