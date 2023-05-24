using System.Linq;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Application.Organization.Dto;
using Application.Organization.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Exceptions;
using Domain.Models.Organization;
using Infrastructure.Database;
using Throw;

namespace Infrastructure.Services;

public class OrganizationService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly IUserRoleService _userRoleService;

	public OrganizationService(ApplicationDbContext dbContext, IMapper mapper, IUserRoleService userRoleService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_userRoleService = userRoleService;
	}

	public async Task<int> CreateAsync(OrganizationCreateDto dto)
	{
		var model = _mapper.Map<OrganizationModel>(dto);

		await _dbContext.Organizations.AddAsync(model);
		await _dbContext.SaveChangesAsync();

		await _userRoleService.AddToRoleAsync(model.Id, dto.OwnerId, RoleName.Owner);

		return model.Id;
	}

	public OrganizationDto Get(int organizationId)
	{
		var organization = _dbContext.Organizations
			.Where(x => x.Id == organizationId)
			.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
			.FirstOrDefault()
			.ThrowIfNull(() => new OrganizationNotFoundException());
		
		return organization;
	}

	public async Task<OrganizationDto> UpdateAsync(OrganizationDto dto)
	{
		var organization = await _dbContext.Organizations.FindAsync(dto.Id);
		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		organization.Name = dto.Name;

		await _dbContext.SaveChangesAsync();

		return dto;
	}

	public async Task DeleteAsync(int organizationId)
	{
		var organization = await _dbContext.Organizations.FindAsync(organizationId);
		organization.ThrowIfNull(() => new OrganizationNotFoundException());
		
		_dbContext.Organizations.Remove(organization);
		await _dbContext.SaveChangesAsync();
	}

	
}