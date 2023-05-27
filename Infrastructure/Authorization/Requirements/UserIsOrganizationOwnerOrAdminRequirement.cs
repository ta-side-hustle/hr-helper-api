using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public class UserIsOrganizationOwnerOrAdminRequirement : IAuthorizationRequirement
{
}