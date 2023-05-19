using System.Net;
using System.Threading.Tasks;
using Api.Modules.Base.Controller;
using Api.Modules.Base.ViewModel;
using Application.Auth.Dto;
using Application.Auth.Interfaces;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Api.Modules.Auth.Controller;

/// <summary>
/// User authentication
/// </summary>
public class AuthController : AppControllerBase
{
	private readonly IAuthService _authService;
	private readonly IHostEnvironment _host;
	private readonly ConnectionStringsOptions _options;

	public AuthController(IAuthService authService, IOptions<ConnectionStringsOptions> options, IHostEnvironment host)
	{
		_authService = authService;
		_host = host;
		_options = options.Value;
	}

	[HttpGet]
	[AllowAnonymous]
	public IActionResult Get()
	{
		return Ok($"{_host.EnvironmentName}: cs: {_options.DefaultConnection}");
	}

	/// <summary>
	/// Authenticate user using credentials
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Authenticate(CredentialDto dto)
	{
		var token = await _authService.Authenticate(dto);
		return new SuccessResult<TokenDto>(token).ToResult(HttpStatusCode.OK);
	}
}