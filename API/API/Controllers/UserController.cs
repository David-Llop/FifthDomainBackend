using API.DataBase.Entities;
using API.DTO.User;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpOptions("login")]
    public IActionResult Options()
    {
        Response.Headers.Append("Allow", "POST, OPTIONS");
        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponseDto>> UserLogin([FromBody] UserLoginDto loginDto)
    {
        User? user = await _userService.GetUserAsync(loginDto.UserName);
        if (user == null)
        {
            return NotFound();
        }
        if (user.Password != loginDto.Password)
        {
            return Unauthorized();
        }
        return Ok(new UserResponseDto { Id = user.Id, UserName = user.Name });
    }
}
