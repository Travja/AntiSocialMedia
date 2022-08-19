using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AntiData.Model;

public class AntiUser : IdentityUser
{
    [Required]
    public string Username { get; set; }
}