using System.Security.Claims;
using Application.Auth.Enums;
using Application.Organization.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Authorization.Handlers;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.UnitTests.Authorization.Handlers;

[TestFixture]
public class UserIsOrganizationOwnerOrAdminHandlerTests
{
	private readonly UserIsOrganizationOwnerOrAdminHandler _sut;
	private readonly IUserRoleService _userRoleService = Substitute.For<IUserRoleService>();

	public UserIsOrganizationOwnerOrAdminHandlerTests()
	{
		_sut = new UserIsOrganizationOwnerOrAdminHandler(_userRoleService);
	}

	[Test]
	public void Constructor_ShouldThrowArgumentNullException_WhenDependencyIsNull()
	{
		var act = () => new UserIsOrganizationOwnerOrAdminHandler(null);

		act.Should().ThrowExactly<ArgumentNullException>();
	}

	[Test]
	[CustomAutoData]
	public async Task HandleAsync_ShouldMarkContextAsFailed_WhenNameIdentifierClaimIsNull(
		UserIsOrganizationOwnerOrAdminRequirement requirement,
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
	public async Task HandleAsync_ShouldMarkContextAsFailed_WhenUserIsNotOrganizationOwner(
		UserIsOrganizationOwnerOrAdminRequirement requirement,
		Guid userId
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, userId.ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInRoleAsync(Arg.Any<int>(), Arg.Is<string>(s => s == userId.ToString()), RoleName.Owner)
			.Returns(false);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		await _sut.HandleAsync(context);

		context.HasFailed.Should().BeTrue();
		context.FailureReasons.Should().HaveCount(1);
	}

	[Test]
	[CustomAutoData]
	public async Task HandleAsync_ShouldMarkContextAsFailed_WhenUserIsNotOrganizationAdmin(
		UserIsOrganizationOwnerOrAdminRequirement requirement,
		Guid userId
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, userId.ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInRoleAsync(Arg.Any<int>(), Arg.Is<string>(s => s == userId.ToString()), RoleName.Admin)
			.Returns(false);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		await _sut.HandleAsync(context);

		context.HasFailed.Should().BeTrue();
		context.FailureReasons.Should().HaveCount(1);
	}

	[Test]
	[CustomAutoData]
	public void HandleAsync_ShouldMarkContextAsSucceeded_WhenUserIsOrganizationOwner(
		UserIsOrganizationOwnerOrAdminRequirement requirement
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInRoleAsync(Arg.Any<int>(), Arg.Any<string>(), RoleName.Owner).Returns(true);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		_sut.HandleAsync(context);

		context.HasSucceeded.Should().BeTrue();
	}

	[Test]
	[CustomAutoData]
	public void HandleAsync_ShouldMarkContextAsSucceeded_WhenUserIsOrganizationAdmin(
		UserIsOrganizationOwnerOrAdminRequirement requirement
	)
	{
		var claim = new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString());
		var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { claim }));
		var resource = new OrganizationModel();
		_userRoleService.IsInRoleAsync(Arg.Any<int>(), Arg.Any<string>(), RoleName.Admin).Returns(true);
		var context = new AuthorizationHandlerContext(new[] { requirement }, user, resource);

		_sut.HandleAsync(context);

		context.HasSucceeded.Should().BeTrue();
	}
}