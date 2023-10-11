using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

namespace Client_App.Services;

/// <summary>
/// Service class for managing token related logic.
/// </summary>
public class TokenService
{
    private readonly IJSRuntime _jSRuntime;

    public TokenService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    /// <summary>
    /// Adds the provided token to local storage
    /// </summary>
    public async Task AddToken(string token)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "token", $"{token}").ConfigureAwait(true);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    /// <summary>
    /// Checks if there is a token in local storage and if it's is still valid.
    /// If valid, token is returned.
    /// If token is not valid the token is removed from local storage and an empty string is returned.
    /// </summary>
    public async Task<string> CheckTokenValid()
    {
        var token = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", "token").ConfigureAwait(true);

        if(!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var jwtToken = jsonToken as JwtSecurityToken;
            DateTime expirationDate = jsonToken.ValidTo.ToLocalTime();
            Console.WriteLine(expirationDate);

            DateTime dateTimeNow = DateTime.Now.ToLocalTime();

            if (dateTimeNow < expirationDate)
            {
                return token;
            }

            await _jSRuntime.InvokeAsync<string>("localStorage.removeItem", "token").ConfigureAwait(true);
        }
        return string.Empty;
    }

    /// <summary>
    /// Reads and extracts the role from the provided token.
    /// Returns the role as a string.
    /// If token is null or empty return an empty string;
    /// </summary>
    public string CheckRole(string token)
    {
        var role = string.Empty;
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var jwtToken = jsonToken as JwtSecurityToken;
            role = jwtToken!.Claims.First(x => x.Type == "role").Value;
        }
        return role;
    }
}
