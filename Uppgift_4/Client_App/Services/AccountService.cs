using System.Net.Http.Json;
using Client_App.Models;

namespace Client_App.Services;

/// <summary>
/// Service class for managing account related logic.
/// </summary>
public class AccountService
{
    private readonly HttpClient _httpClient;
    private readonly string _url = "https://localhost:7235/login";

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Method for logging in the user.
    /// A post request is made with the set url and the userinformation, UserDto with parameters email and password.
    /// Returns the response from the Api
    /// </summary>
    public async Task<HttpResponseMessage> Login(UserDto user)
    {
        return await _httpClient.PostAsJsonAsync(_url, user);
    }
}
