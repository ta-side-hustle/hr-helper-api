using System.Threading.Tasks;
using Application.Organization.Interfaces;
using Application.User.Dto;
using Application.User.Interfaces;
using Domain.Exceptions;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Throw;

namespace Infrastructure.Services;

public class UserService : IUserService
{
	private readonly UserManager<UserModel> _userManager;
	private readonly IUserRoleService _roleService;

	public UserService(UserManager<UserModel> userManager, IUserRoleService roleService)
	{
		_userManager = userManager;
		_roleService = roleService;
	}

	public async Task<string> Create(UserCreateDto dto)
	{
		var user = new UserModel
		{
			LastName = dto.LastName,
			FirstName = dto.FirstName,
			Email = dto.Email,
			UserName = dto.Email
		};

		var result = await _userManager.CreateAsync(user, dto.Password);

		result.Throw(() => new IdentityException(result.Errors))
			.IfFalse(identityResult => identityResult.Succeeded);

		return user.Id;
	}

	public async Task<UserDto> Get(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		user.ThrowIfNull(() => new UserNotFoundException());

		var userOrganizations = await _roleService.GetAsync(id);
		
		return new UserDto
		{
			LastName = user.LastName,
			FirstName = user.FirstName,
			Email = user.Email,
			Organizations = userOrganizations
		};
	}

	public async Task<bool> Delete(string id)
	{
		var user = await _userManager.FindByIdAsync(id);
		user.ThrowIfNull(() => new UserNotFoundException());

		var result = await _userManager.DeleteAsync(user);

		return result.Succeeded;
	}

	public async Task<UserDto> Update(string id, UserDto dto)
	{
		var user = await _userManager.FindByIdAsync(id);
		user.ThrowIfNull(() => new UserNotFoundException());

		user.LastName = dto.LastName;
		user.FirstName = dto.FirstName;
		
		var result = await _userManager.UpdateAsync(user);

		result.Throw(() => new IdentityException(result.Errors))
			.IfFalse(identityResult => identityResult.Succeeded);

		return dto;
	}
}