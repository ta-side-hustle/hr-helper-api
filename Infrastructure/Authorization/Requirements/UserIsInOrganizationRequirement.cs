using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public class UserIsInOrganizationRequirement : IAuthorizationRequirement
{
}