#nullable enable
using System.Security.Principal;
using Application.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Providers;

public class PrincipalProvider : IPrincipalProvider
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	public PrincipalProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public IPrincipal? Principal => _httpContextAccessor.HttpContext?.User;
}