using System.Security.Claims;
using Application.Organization.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Authorization.Handlers;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.UnitTests.Authorization.Handlers;

[TestFixture]
public class UserIsInOrganizationHandlerTests
{
	private readonly UserIsInOrganizationHandler _sut;
	private readonly IUserRoleService _userRoleService = Substitute.For<IUserRoleService>();

	public UserIsInOrganizationHandlerTests()
	{
		_sut = new UserIsInOrganizationHandler(_userRoleService);
	}
	
	[Test]
	public void Constructor_ShouldThrowArgumentNullException_WhenDependencyIsNull()
	{
		var act = () => new UserIsInOrganizationHandler(null);

		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[Test]
	[CustomAutoData]
	public async Task HandleAsync_ShouldMarkContextAsFailed_WhenNameIdentifierClaimIsNull(
		UserIsInOrganizationRequirement requirement,
		ClaimsPrincipal user
	)
	{
		var resource = new OrganizationModel();
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		await _sut.HandleAsync(context);

		context.HasFailed.Should().BeTrue();
		context.FailureReasons.Should().HaveCount(1);
	}

	[Test]
	[CustomAutoData]
	public async Task HandleAsync_ShouldMarkContextAsFailed_WhenUserIsNotInOrganization(
		UserIsInOrganizationRequirement requirement,
		Guid userId
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, userId.ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInOrganizationAsync(Arg.Any<int>(), Arg.Is<string>(s => s == userId.ToString()))
			.Returns(false);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		await _sut.HandleAsync(context);

		context.HasFailed.Should().BeTrue();
		context.FailureReasons.Should().HaveCount(1);
	}

	[Test]
	[CustomAutoData]
	public void HandleAsync_ShouldMarkContextAsSucceeded_WhenRequirementIsSatisfied(
		UserIsInOrganizationRequirement requirement
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInOrganizationAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(true);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		_sut.HandleAsync(context);

		context.HasSucceeded.Should().BeTrue();
	}
}