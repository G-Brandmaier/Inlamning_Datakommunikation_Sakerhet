using Api_Hub.Models;
using Api_Hub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Hub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    /// <summary>
    /// Dependency injection of AccountService.
    /// AccountService takes care of the authentication of the user.
    /// </summary>
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Excectution of the log in and returning of authentication token.
    /// Takes a UserDto for reference with the data.
    /// Returns token if log in is successfull otherwise Unauthorized or BadRequest.s
    /// </summary>
    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserDto user)
    {
        if (ModelState.IsValid)
        {
            var token = _accountService.Login(user);
            if(string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
        return BadRequest();
    }

}
