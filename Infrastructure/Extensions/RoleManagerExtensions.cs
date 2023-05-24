using System;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Extensions;

public static class RoleManagerExtensions
{
	public static async Task<RoleModel> Find(this RoleManager<RoleModel> roleManager, RoleName role)
	{
		var roleName = Enum.GetName(typeof(RoleName), role);

		ArgumentException.ThrowIfNullOrEmpty(roleName, nameof(roleName));

		return await roleManager?.FindByNameAsync(roleName)!;
	}
}