using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Auth.Enums;
using Application.Organization.Interfaces;
using Application.User.Dto;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Auth;
using Domain.Models.Organization;
using Infrastructure.Database;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Infrastructure.Services;

public class UserRoleService : IUserRoleService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly RoleManager<RoleModel> _roleManager;
	private readonly UserManager<UserModel> _userManager;

	public UserRoleService(ApplicationDbContext dbContext, RoleManager<RoleModel> roleManager,
		UserManager<UserModel> userManager, IMapper mapper)
	{
		_dbContext = dbContext;
		_roleManager = roleManager;
		_userManager = userManager;
		_mapper = mapper;
	}

	public async Task<bool> IsInRoleAsync(int organizationId, string userId, RoleName roleName)
	{
		var user = await _dbContext.Users
			.Include(x => x.Organizations)
			.FirstOrDefaultAsync(x => x.Id == userId);

		user.ThrowIfNull(() => new UserNotFoundException());

		var role = await _roleManager.Find(roleName);

		return user.Organizations.Any(x => x.OrganizationId == organizationId && x.RoleId == role.Id);
	}

	public async Task<bool> IsInOrganizationAsync(int organizationId, string userId)
	{
		var user = await _dbContext.Users
			.Include(x => x.Organizations)
			.FirstOrDefaultAsync(x => x.Id == userId);

		user.ThrowIfNull(() => new UserNotFoundException());

		return user.Organizations.Any(x => x.OrganizationId == organizationId);
	}

	public async Task AddToRoleAsync(int organizationId, string userId, RoleName roleName)
	{
		var organization = _dbContext.Organizations
			.Include(x => x.Users)
			.FirstOrDefault(x => x.Id == organizationId);
		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		var user = await _userManager.FindByIdAsync(userId);
		user.ThrowIfNull(() => new UserNotFoundException());

		var ownerRole = await _roleManager.Find(RoleName.Owner);
		var organizationHasOwner = organization.Users.Any(x => x.RoleId == ownerRole.Id);

		(roleName == RoleName.Owner && organizationHasOwner)
			.Throw(() => new OrganizationRoleException("У организации может быть только один владелец"))
			.IfTrue();

		(await IsInRoleAsync(organizationId, userId, roleName))
			.Throw(() => new OrganizationRoleException("У пользователя уже есть такая роль"))
			.IfTrue();

		var role = await _roleManager.Find(roleName);
		UserOrganizationModel userOrganizationModel = new()
		{
			User = user,
			Role = role,
			Organization = organization
		};

		await _dbContext.UserOrganizations.AddAsync(userOrganizationModel);
		await _dbContext.SaveChangesAsync();
	}

	public async Task RemoveFromRoleAsync(int organizationId, string userId, RoleName roleName)
	{
		var organization = _dbContext.Organizations
			.Include(x => x.Users)
			.FirstOrDefault(x => x.Id == organizationId);
		organization.ThrowIfNull(() => new OrganizationNotFoundException());

		var user = await _userManager.FindByIdAsync(userId);
		user.ThrowIfNull(() => new UserNotFoundException());

		(await IsInRoleAsync(organizationId, userId, RoleName.Owner))
			.Throw(() => new OrganizationRoleException("Для смены владельца воспользуйтеся передачей прав"))
			.IfTrue();

		var role = await _roleManager.Find(roleName);
		var userRoleToDelete = _dbContext.UserOrganizations
			.FirstOrDefault(x => x.OrganizationId == organizationId && x.UserId == userId && x.RoleId == role.Id);

		userRoleToDelete.ThrowIfNull(() => new OrganizationRoleException("У пользователя нет такой роли"));

		_dbContext.UserOrganizations.Remove(userRoleToDelete);
		await _dbContext.SaveChangesAsync();
	}

	public async Task<IList<UserOrganizationDto>> GetAsync(string userId)
	{
		var userOrganizations = await _dbContext.UserOrganizations
			.Include(x => x.Organization)
			.Include(x => x.Role)
			.Where(x => x.UserId == userId)
			.ToListAsync();

		return _mapper.Map<IEnumerable<UserOrganizationDto>>(userOrganizations).ToList();
	}

	public async Task<UserOrganizationDto> GetAsync(string userId, int organizationId)
	{
		var userIsInOrganization = await IsInOrganizationAsync(organizationId, userId);

		userIsInOrganization
			.Throw(() =>
				new OrganizationNotFoundException(
					"Организация не существует или пользователь не является её сотрудником"))
			.IfFalse();

		var userOrganization = await _dbContext.UserOrganizations
			.Include(x => x.Organization)
			.Include(x => x.Role)
			.Where(x => x.UserId == userId && x.OrganizationId == organizationId)
			.ToListAsync();

		return _mapper.Map<IEnumerable<UserOrganizationDto>>(userOrganization).First();
	}
}