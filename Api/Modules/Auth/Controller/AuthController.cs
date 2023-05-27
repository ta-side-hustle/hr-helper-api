using System.Net;
using System.Threading.Tasks;
using Api.Modules.Base.Controller;
using Api.Modules.Base.ViewModel;
using Application.Auth.Dto;
using Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Modules.Auth.Controller;

/// <summary>
/// User authentication
/// </summary>
public class AuthController : AppControllerBase
{
	private readonly IAppAuthenticationService _authenticationService;

	public AuthController(IAppAuthenticationService authenticationService)
	{
		_authenticationService = authenticationService;
	}

	/// <summary>
	/// Authenticate user using credentials
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	[HttpPost]
	[AllowAnonymous]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<TokenDto>))]
	[SwaggerResponse((int)HttpStatusCode.BadRequest, type: typeof(ErrorResult))]
	public async Task<IActionResult> Authenticate(CredentialDto dto)
	{
		var token = await _authenticationService.Authenticate(dto);
		return new SuccessResult<TokenDto>(token).ToResult(HttpStatusCode.OK);
	}
}