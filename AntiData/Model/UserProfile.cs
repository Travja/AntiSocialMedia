using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AntiData.Model;

public class UserProfile
{
    public int Id { get; set; } = 0;
    [Display(Name = "Name")]
    [StringLength(32, ErrorMessage = "Your name must be at least 3 characters and cannot be longer than 32 characters", MinimumLength = 3)]
    public string Name { get; set; } = "No Name";
    [Display(Name = "Age")]
    [Range(5, 130, ErrorMessage = "You must be between 5 and 130 years old")]
    public int Age { get; set; } = -1;
    [Display(Name = "Favorite Food")]
    public string? FavoriteFood { get; set; }
    [Display(Name = "Favorite Book")]
    public string? FavoriteBook { get; set; }
}