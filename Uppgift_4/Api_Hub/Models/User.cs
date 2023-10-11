using System.ComponentModel.DataAnnotations;

namespace Api_Hub.Models;

public class User
{
    public Guid Id { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(nameof(Password))]
    public string Password { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
}
