using System.Reflection;
using Application.Auth.Interfaces;
using Application.Organization.Interfaces;
using Application.Organization.Mapper;
using Application.User.Interfaces;
using Domain.Models.Auth;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class InfrastructureServices
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>();

		services.AddIdentityCore<UserModel>(options =>
			{
				options.Password.RequiredLength = 4;
				options.Password.RequireDigit = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;

				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;

				options.User.RequireUniqueEmail = true;
			})
			.AddRoles<RoleModel>()
			.AddEntityFrameworkStores<ApplicationDbContext>();

		services.AddAuthorizationPolicies();

		services.AddAutoMapper(Assembly.GetAssembly(typeof(OrganizationMappingProfile)));

		services
			.AddScoped<IAppAuthenticationService, AppAuthenticationService>()
			.AddScoped<IAppAuthorizationService, AppAuthorizationService>()
			.AddScoped<IUserService, UserService>()
			.AddScoped<IUserRoleService, UserRoleService>()
			.AddScoped<IOrganizationService, OrganizationService>()
			;

		return services;
	}
}