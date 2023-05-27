using System;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Auth.Interfaces;
using Infrastructure.Authorization.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Throw;

namespace Infrastructure.Services;

public class AppAuthorizationService : IAppAuthorizationService
{
	private readonly IAuthorizationService _authorizationService;
	private readonly IPrincipalProvider _principalProvider;

	public AppAuthorizationService(IAuthorizationService authorizationService, IPrincipalProvider principalProvider)
	{
		_authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
		_principalProvider = principalProvider ?? throw new ArgumentNullException(nameof(principalProvider));
	}
	
	public async Task AuthorizeAsync(object resource, string policy)
	{
		if (_principalProvider.Principal is not ClaimsPrincipal principal) throw new AuthenticationException();
		
		var authorizationResult = await _authorizationService.AuthorizeAsync(principal, resource, policy);

		authorizationResult.Succeeded
			.Throw(() => new AuthorizationException(authorizationResult.Failure))
			.IfFalse();
	}
}