using Application.Auth.Interfaces;
using Application.User.Interfaces;
using Domain.Models.Auth;
using Infrastructure.Auth.Service;
using Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
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

			services
				.AddScoped<IAuthService, AuthService>()
				.AddScoped<IUserService, UserService>()
				;

			return services;
		}
	}
}