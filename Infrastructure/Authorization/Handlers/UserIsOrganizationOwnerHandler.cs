using System.Security.Claims;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Application.Organization.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Handlers;

public class
	UserIsOrganizationOwnerHandler : AuthorizationHandler<UserIsOrganizationOwnerRequirement, OrganizationModel>
{
	private readonly IUserRoleService _userRoleService;

	public UserIsOrganizationOwnerHandler(IUserRoleService userRoleService)
	{
		_userRoleService = userRoleService;
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		UserIsOrganizationOwnerRequirement requirement,
		OrganizationModel resource)
	{
		var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

		if (userId == null)
		{
			context.Fail(new AuthorizationFailureReason(this, "Пользователь не аутентифицирован"));
			return;
		}

		var userInOrganizationOwner = await _userRoleService.IsInRoleAsync(resource.Id, userId.Value, RoleName.Owner);

		if (userInOrganizationOwner)
			context.Succeed(requirement);
		else
			context.Fail(new AuthorizationFailureReason(this,
				"Для выполнения этого действия вы должны быть владельцем организации"));
	}
}