using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Application.Auth.Interfaces;
using Infrastructure.Authorization.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.UnitTests.Services;

[TestFixture]
public class AppAuthorizationServiceTests
{
	private readonly IAppAuthorizationService _sut;
	private readonly IAuthorizationService _authorizationService = Substitute.For<IAuthorizationService>();
	private readonly IPrincipalProvider _principalProvider = Substitute.For<IPrincipalProvider>();

	public AppAuthorizationServiceTests()
	{
		_authorizationService
			.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object>(), Arg.Any<string>())
			.Returns(AuthorizationResult.Success());

		_sut = new AppAuthorizationService(_authorizationService, _principalProvider);
	}

	[Test]
	public void Constructor_ShouldThrowArgumentNullException_WhenIPrincipalProviderIsNull()
	{
		var act = () => new AppAuthorizationService(_authorizationService, null);

		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[Test]
	public void Constructor_ShouldThrowArgumentNullException_WhenIAuthorizationServiceIsNull()
	{
		var act = () => new AppAuthorizationService(null, _principalProvider);

		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[Test]
	[CustomAutoData]
	public async Task AuthorizeAsync_ShouldReturnTask_WhenIPrincipalIsClaimsPrincipal(
		ClaimsPrincipal claimsPrincipal,
		object resource,
		string policy
	)
	{
		_principalProvider.Principal.Returns(claimsPrincipal);

		var act = () => _sut.AuthorizeAsync(resource, policy);

		await act.Should().NotThrowAsync();
		act().Should().Be(Task.CompletedTask);
	}

	[Test]
	[CustomAutoData]
	public async Task AuthorizeAsync_ShouldBeCalledOnce_WhenIPrincipalIsClaimsPrincipal(
		ClaimsPrincipal claimsPrincipal,
		object resource,
		string policy
	)
	{
		_principalProvider.Principal.Returns(claimsPrincipal);

		await _sut.AuthorizeAsync(resource, policy);

		await _authorizationService.Received().AuthorizeAsync(
			Arg.Is<ClaimsPrincipal>(c => c == claimsPrincipal),
			Arg.Is<object>(o => o == resource),
			Arg.Is<string>(s => s == policy));
	}

	[Test]
	[CustomAutoData]
	public async Task AuthorizeAsync_ShouldThrowAuthenticationException_WhenIPrincipalIsNotClaimsPrincipal(
		object? nonPrincipal,
		object resource,
		string policy
	)
	{
		_principalProvider.Principal.Returns(nonPrincipal as IPrincipal);

		var act = () => _sut.AuthorizeAsync(resource, policy);

		await act.Should().ThrowExactlyAsync<AuthenticationException>();
	}

	[Test]
	[CustomAutoData]
	public async Task AuthorizeAsync_ShouldThrowAuthorizationException_WhenAuthorizationResultIsNotSuccessful(
		ClaimsPrincipal claimsPrincipal,
		object resource,
		string policy
	)
	{
		_principalProvider.Principal.Returns(claimsPrincipal);
		_authorizationService.AuthorizeAsync(claimsPrincipal, resource, policy).Returns(AuthorizationResult.Failed());

		var act = () => _sut.AuthorizeAsync(resource, policy);

		await act.Should().ThrowExactlyAsync<AuthorizationException>();
	}
}