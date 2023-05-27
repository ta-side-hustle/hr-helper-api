using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Infrastructure.Services;

public class OrganizationService : IOrganizationService
{
	private readonly IAppAuthorizationService _appAuthorizationService;
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IUserRoleService _userRoleService;

	public OrganizationService(ApplicationDbContext dbContext, IMapper mapper, IUserRoleService userRoleService,
		IAppAuthorizationService appAuthorizationService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_userRoleService = userRoleService;
		_appAuthorizationService = appAuthorizationService;
	}

	public async Task<int> CreateAsync(OrganizationCreateDto dto, string ownerId)
	{
		var model = _mapper.Map<OrganizationModel>(dto);

		await _dbContext.Organizations.AddAsync(model);
		await _dbContext.SaveChangesAsync();

		await _userRoleService.AddToRoleAsync(model.Id, ownerId, RoleName.Owner);

		return model.Id;
	}

	public async Task<OrganizationDto> GetAsync(int organizationId)
	{
		var organization = _dbContext.Organizations
			.FirstOrDefault(x => x.Id == organizationId);

		await _appAuthorizationService.AuthorizeAsync(organization, AuthorizationPolicies.UserIsEmployee);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		return _mapper.Map<OrganizationDto>(organization);
	}

	public async Task<IList<OrganizationDto>> GetAllByUserAsync(string userId)
	{
		var organizations = _dbContext.Organizations
			.Include(x => x.Users)
			.Where(x => x.Users.Any(u => u.UserId == userId))
			.ToList();

		organizations
			.ThrowIfNull(() => new OrganizationNotFoundException())
			.IfEmpty();

		return await Task.FromResult(_mapper.Map<List<OrganizationDto>>(organizations));
	}

	public async Task<OrganizationDto> UpdateAsync(OrganizationDto dto)
	{
		var organization = await _dbContext.Organizations.FindAsync(dto.Id);

		await _appAuthorizationService.AuthorizeAsync(organization, AuthorizationPolicies.UserIsOwnerOrAdmin);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		organization.Name = dto.Name;

		await _dbContext.SaveChangesAsync();

		return dto;
	}

	public async Task DeleteAsync(int organizationId)
	{
		var organization = await _dbContext.Organizations.FindAsync(organizationId);

		await _appAuthorizationService.AuthorizeAsync(organization, AuthorizationPolicies.UserIsOwner);

		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		_dbContext.Organizations.Remove(organization);
		await _dbContext.SaveChangesAsync();
	}
}