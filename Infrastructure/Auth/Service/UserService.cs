using System.Threading.Tasks;
using Application.User.Dto;
using Application.User.Interfaces;
using Domain.Exceptions;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Throw;

namespace Infrastructure.Auth.Service;

public class UserService : IUserService
{
	private readonly UserManager<UserModel> _userManager;

	public UserService(UserManager<UserModel> userManager)
	{
		_userManager = userManager;
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

		return new UserDto
		{
			LastName = user.LastName,
			FirstName = user.FirstName,
			Email = user.Email,
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