using System.Linq;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Application.Auth.Interfaces;
using Application.Organization.Dto;
using Application.Organization.Interfaces;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Organization;
using Infrastructure.Authorization;
using Infrastructure.Database;
using Throw;

namespace Infrastructure.Services;

public class OrganizationService : IOrganizationService
{
	private readonly IAuthorizationGuard _authorizationGuard;
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IUserRoleService _userRoleService;

	public OrganizationService(ApplicationDbContext dbContext, IMapper mapper, IUserRoleService userRoleService,
		IAuthorizationGuard authorizationGuard)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_userRoleService = userRoleService;
		_authorizationGuard = authorizationGuard;
	}

	public async Task<int> CreateAsync(OrganizationCreateDto dto)
	{
		var model = _mapper.Map<OrganizationModel>(dto);

		await _dbContext.Organizations.AddAsync(model);
		await _dbContext.SaveChangesAsync();

		await _userRoleService.AddToRoleAsync(model.Id, dto.OwnerId, RoleName.Owner);

		return model.Id;
	}

	public async Task<OrganizationDto> GetAsync(int organizationId)
	{
		var organization = _dbContext.Organizations
			.FirstOrDefault(x => x.Id == organizationId);

		await _authorizationGuard.AuthorizeAsync(organization, AuthorizationPolicies.UserIsEmployee);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		return _mapper.Map<OrganizationDto>(organization);
	}

	public async Task<OrganizationDto> UpdateAsync(OrganizationDto dto)
	{
		var organization = await _dbContext.Organizations.FindAsync(dto.Id);

		await _authorizationGuard.AuthorizeAsync(organization, AuthorizationPolicies.UserIsOwnerOrAdmin);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		organization.Name = dto.Name;

		await _dbContext.SaveChangesAsync();

		return dto;
	}

	public async Task DeleteAsync(int organizationId)
	{
		var organization = await _dbContext.Organizations.FindAsync(organizationId);

		await _authorizationGuard.AuthorizeAsync(organization, AuthorizationPolicies.UserIsOwner);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		_dbContext.Organizations.Remove(organization);
		await _dbContext.SaveChangesAsync();
	}
}