using Cards.Application.Common.Models;
using Cards.Application.Users.Commands;
using Cards.Application.Users.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Api.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AccessTokenVm>>> Login([FromBody] LoginCommand command)
    {
        return CustomResponse(await Mapper.Send(command));
    }
    
    [HttpPost("seed-users")]
    public async Task<ActionResult<ApiResponse<UserVm[]>>> SeedUsers()
    {
        return CustomResponse(await Mapper.Send(new SeedUsersCommand()));
    }
}