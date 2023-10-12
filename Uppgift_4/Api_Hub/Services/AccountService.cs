using Api_Hub.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Hub.Services;

/// <summary>
/// Service class for handeling login validation and generating token.
/// </summary>
public class AccountService
{
    private List<User> Users { get; set; }
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Setting the list of valid users in the constructor.
    /// </summary>

    public AccountService(IConfiguration configuration)
    {
        Users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Password = "Test123!",
                Role = "admin"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "test1@test.com",
                Password = "Testar123!",
                Role = "user"
            }
        };
        _configuration = configuration;
    }

    /// <summary>
    /// Execute the login logic, validate if the information provided in UserDto is matching any from the list.
    /// If validation succeeded Claims are set and a token is generated with these calims.
    /// Returns token if validation succeeded, otherwise returns null.
    /// </summary>

    public string Login(UserDto user)
    {
        if(user != null)
        {
            var result = Users.Where(x => x.Email == user.Email && x.Password == user.Password).SingleOrDefault();

            if (result != null)
            {
                var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                new Claim("id", result.Id.ToString()),
                    new Claim(ClaimTypes.Name, result.Email!),
                    new Claim(ClaimTypes.Role, result.Role)
                });
                return GenerateToken(claimsIdentity, DateTime.Now.AddHours(2));
            }
        }
        return null;
    }

    /// <summary>
    /// Generating authentication token.
    /// Sets the calims and expire date received and generates token for authenticated user.
    /// Returns token.
    /// </summary>

    private string GenerateToken(ClaimsIdentity claimsIdentity, DateTime expiresAt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration.GetSection("Jwt")["Issuer"],
            Audience = _configuration.GetSection("Jwt")["Audience"],
            Subject = claimsIdentity,
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt")["Key"]!)),
                SecurityAlgorithms.HmacSha512Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(securityTokenDescriptor));
    }
}
