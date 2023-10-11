using System.ComponentModel.DataAnnotations;

namespace Client_App.Models;

public class UserDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Not a valid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(nameof(Password))]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$,", ErrorMessage = "Not a valid password")]
    public string Password { get; set; } = null!;

}
