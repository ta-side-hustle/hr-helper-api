using System.Threading.Tasks;
using Application.Auth.Enums;
using Domain.Exceptions;

namespace Application.Organization.Interfaces;

public interface IUserRoleService
{
	/// <summary>
	///     Check if the specified user is in role in the specified organization.
	/// </summary>
	/// <param name="organizationId">Id of organization to check against.</param>
	/// <param name="userId">Id of the user to check.</param>
	/// <param name="roleName">Name of the role to check.</param>
	/// <returns>
	///     The <see cref="Task" /> that represents the asynchronous operation, containing a flag indicating whether the
	///     specified <paramref name="userId" /> is
	///     a member of the role in specified <paramref name="organizationId" />.
	/// </returns>
	/// <exception cref="UserNotFoundException">User is not found.</exception>
	Task<bool> IsInRoleAsync(int organizationId, string userId, RoleName roleName);

	/// <summary>
	///     Check if the specified user is in the specified organization.
	/// </summary>
	/// <param name="organizationId">Id of organization to check against.</param>
	/// <param name="userId">Id of the user to check.</param>
	/// <returns>
	///     The <see cref="Task" /> that represents the asynchronous operation, containing a flag indicating whether the
	///     specified <paramref name="userId" /> is a member of the specified organization.
	/// </returns>
	/// <exception cref="UserNotFoundException">User is not found.</exception>
	Task<bool> IsInOrganizationAsync(int organizationId, string userId);

	/// <summary>
	///     Add the specified <paramref name="userId" /> to the named role.
	/// </summary>
	/// <param name="organizationId">Id of the organization in which specified userId will be added to the role.</param>
	/// <param name="userId">Id of the user to add to the role.</param>
	/// <param name="roleName">Name of the role to add the user to.</param>
	/// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
	/// <exception cref="OrganizationNotFoundException">Organization is not found.</exception>
	/// <exception cref="UserNotFoundException">User is not found.</exception>
	/// <exception cref="OrganizationRoleException">Owner is already set or the user is already in the role.</exception>
	Task AddToRoleAsync(int organizationId, string userId, RoleName roleName);

	/// <summary>
	///     Removes the specified user from the role in the specified organization.
	/// </summary>
	/// <param name="organizationId">Id of the organization from which specified user role will be deleted.</param>
	/// <param name="userId">Id of the user to add to the role.</param>
	/// <param name="roleName">Name of the role to add the user to.</param>
	/// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
	/// <exception cref="OrganizationNotFoundException">Organization is not found.</exception>
	/// <exception cref="UserNotFoundException">User is not found.</exception>
	/// <exception cref="OrganizationRoleException">User is owner or not in the role.</exception>
	Task RemoveFromRoleAsync(int organizationId, string userId, RoleName roleName);
}