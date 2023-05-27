using Infrastructure.Authorization.Handlers;
using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization;

public static class AuthorizationPolicies
{
	public const string UserIsOwner = "UserIsOwnerPolicy";
	public const string UserIsOwnerOrAdmin = "UserIsOwnerOrAdminPolicy";
	public const string UserIsEmployee = "UserIsEmployeePolicy";

	public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
	{
		return services
			.AddTransient<IAuthorizationHandler, UserIsInOrganizationHandler>()
			.AddTransient<IAuthorizationHandler, UserIsOrganizationOwnerHandler>()
			.AddTransient<IAuthorizationHandler, UserIsOrganizationOwnerOrAdminHandler>()
			.AddAuthorizationCore(options =>
			{
				options.AddPolicy(UserIsOwner, policy =>
					policy
						.RequireAuthenticatedUser()
						.AddRequirements(new UserIsOrganizationOwnerRequirement()));

				options.AddPolicy(UserIsOwnerOrAdmin, policy =>
					policy
						.RequireAuthenticatedUser()
						.AddRequirements(new UserIsOrganizationOwnerOrAdminRequirement()));

				options.AddPolicy(UserIsEmployee, policy =>
					policy
						.RequireAuthenticatedUser()
						.AddRequirements(new UserIsInOrganizationRequirement()));
			});
	}
}