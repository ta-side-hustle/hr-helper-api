using System.Security.Claims;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Application.Organization.Interfaces;
using Domain.Models.Organization;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Handlers;

public class
	UserIsOrganizationOwnerOrAdminHandler : AuthorizationHandler<UserIsOrganizationOwnerOrAdminRequirement,
		OrganizationModel>
{
	private readonly IUserRoleService _userRoleService;

	public UserIsOrganizationOwnerOrAdminHandler(IUserRoleService userRoleService)
	{
		_userRoleService = userRoleService;
	}

	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		UserIsOrganizationOwnerOrAdminRequirement requirement,
		OrganizationModel resource)
	{
		var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);

		if (userId == null)
		{
			context.Fail(new AuthorizationFailureReason(this, "Пользователь не аутентифицирован"));
			return;
		}

		var userInOrganizationOwner = await _userRoleService.IsInRoleAsync(resource.Id, userId.Value, RoleName.Owner);
		var userInOrganizationAdmin = await _userRoleService.IsInRoleAsync(resource.Id, userId.Value, RoleName.Admin);

		if (userInOrganizationOwner || userInOrganizationAdmin)
			context.Succeed(requirement);
		else
			context.Fail(new AuthorizationFailureReason(this,
				"Для выполнения этого действия вы должны быть администратором организации"));
	}
}