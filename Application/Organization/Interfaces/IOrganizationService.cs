using System.Security.Authentication;
using System.Threading.Tasks;
using Application.Organization.Dto;
using Domain.Exceptions;

namespace Application.Organization.Interfaces;

public interface IOrganizationService
{
	/// <summary>
	///     Create new organization.
	/// </summary>
	/// <param name="dto">Organization data.</param>
	/// <returns>
	///     The <see cref="Task" /> that represents the asynchronous operation, containing an id of the created
	///     organization.
	/// </returns>
	Task<int> CreateAsync(OrganizationCreateDto dto, string ownerId);

	/// <summary>
	///     Get data of the organization.
	/// </summary>
	/// <param name="organizationId">Id of the organization.</param>
	/// <returns>
	///     The <see cref="Task" /> that represents the asynchronous operation, containing organization data.
	/// </returns>
	/// <exception cref="OrganizationNotFoundException">Specified organization not found.</exception>
	/// <exception cref="AuthenticationException">User is not authenticated.</exception>
	Task<OrganizationDto> GetAsync(int organizationId);

	/// <summary>
	///     Update data of the organization.
	/// </summary>
	/// <param name="dto">Organization data.</param>
	/// <returns>
	///     The <see cref="Task" /> that represents the asynchronous operation, containing updated organization data.
	/// </returns>
	/// <exception cref="OrganizationNotFoundException">Specified organization not found.</exception>
	/// <exception cref="AuthenticationException">User is not authenticated.</exception>
	Task<OrganizationDto> UpdateAsync(OrganizationDto dto);

	/// <summary>
	///     Delete specified organization.
	/// </summary>
	/// <param name="organizationId">Id of the organization to delete.</param>
	/// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
	/// <exception cref="OrganizationNotFoundException">Specified organization not found.</exception>
	/// <exception cref="AuthenticationException">User is not authenticated.</exception>
	Task DeleteAsync(int organizationId);
}