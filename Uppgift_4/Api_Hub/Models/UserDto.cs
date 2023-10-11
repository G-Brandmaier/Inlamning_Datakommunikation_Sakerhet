using System.ComponentModel.DataAnnotations;

namespace Api_Hub.Models;

public class UserDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(nameof(Password))]
    public string Password { get; set; } = null!;

}
