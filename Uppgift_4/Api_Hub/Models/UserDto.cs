using System.ComponentModel.DataAnnotations;

namespace Api_Hub.Models;

/// <summary>
/// Is used for receiving data when a post request for login is called.
/// </summary>
public class UserDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(nameof(Password))]
    public string Password { get; set; } = null!;

}
