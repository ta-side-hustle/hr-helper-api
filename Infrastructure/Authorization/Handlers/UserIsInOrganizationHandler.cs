using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Organization.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Handlers;

public class UserIsInOrganizationHandler : AuthorizationHandler<UserIsInOrganizationRequirement, OrganizationModel>
{
	private readonly IUserRoleService _userRoleService;

	public UserIsInOrganizationHandler(IUserRoleService userRoleService)
	{
		_userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		UserIsInOrganizationRequirement requirement,
		OrganizationModel resource)
	{
		var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

		if (userId == null)
		{
			context.Fail(new AuthorizationFailureReason(this, "Пользователь не аутентифицирован"));
			return;
		}

		var userInOrganization = await _userRoleService.IsInOrganizationAsync(resource.Id, userId.Value);

		if (userInOrganization)
			context.Succeed(requirement);
		else
			context.Fail(new AuthorizationFailureReason(this, "Пользователь не является членом организации"));
	}
}