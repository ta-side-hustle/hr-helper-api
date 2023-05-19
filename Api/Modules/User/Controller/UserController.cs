using System.Net;
using System.Threading.Tasks;
using Api.Modules.Base.Controller;
using Api.Modules.Base.ViewModel;
using Application.User.Dto;
using Application.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Modules.User.Controller;

/// <summary>
/// User account management
/// </summary>
[ApiController]
public class UserController : AppControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	/// <summary>
	/// Get current user
	/// </summary>
	/// <returns></returns>
	[HttpGet("")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<UserDto>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> GetUser()
	{
		var user = await _userService.Get(UserId);
		return new SuccessResult<UserDto>(user).ToResult(HttpStatusCode.OK);
	}

	/// <summary>
	/// Register new user account
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	[AllowAnonymous]
	[HttpPost("")]
	[SwaggerResponse((int)HttpStatusCode.Created, type: typeof(ResourceCreateResult<string>))]
	public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
	{
		var result = await _userService.Create(dto);
		return new ResourceCreateResult<string>(result).ToResult(HttpStatusCode.Created);
	}

	/// <summary>
	/// Update existing user account
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	[HttpPut("")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<UserDto>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> UpdateUser([FromBody] UserDto dto)
	{
		var result = await _userService.Update(UserId, dto);
		return new SuccessResult<UserDto>(result).ToResult(HttpStatusCode.OK);
	}

	/// <summary>
	/// Delete user account
	/// </summary>
	/// <returns></returns>
	[HttpDelete("")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<UserDto>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> DeleteUser()
	{
		if (await _userService.Delete(UserId))
		{
			return NoContent();
		}

		return new ErrorResult("При удалении пользователя произошла ошибка").ToResult(HttpStatusCode.InternalServerError);
	}
}