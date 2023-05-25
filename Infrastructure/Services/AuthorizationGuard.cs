using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Auth.Interfaces;
using Infrastructure.Authorization.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Throw;

namespace Infrastructure.Services;

public class AuthorizationGuard : IAuthorizationGuard
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IPrincipalProvider _principalProvider;

	public AuthorizationGuard(IAuthorizationService authorizationService, IPrincipalProvider principalProvider)
	{
		_authorizationService = authorizationService;
		_principalProvider = principalProvider;
	}
	
	public async Task AuthorizeAsync(object resource, string policy)
	{
		if (_principalProvider.Principal is not ClaimsPrincipal principal) throw new AuthenticationException();
		
		var authorizationResult = await _authorizationService.AuthorizeAsync(principal, resource, policy);

		authorizationResult.Succeeded
			.Throw(() => new AuthorizationException(authorizationResult.Failure))
			.IfFalse();

		throw new AuthenticationException();
	}
}