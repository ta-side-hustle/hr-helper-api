using System.Threading.Tasks;
using Application.User.Dto;
using Domain.Exceptions;

namespace Application.User.Interfaces;

public interface IUserService
{
	/// <summary>
	/// Creates a new user
	/// </summary>
	/// <param name="dto">User data</param>
	/// <returns>Id of created user</returns>
	/// <exception cref="IdentityException">Save operation wasn't successful</exception>
	Task<string> Create(UserCreateDto dto);
	
	/// <summary>
	/// Get user by id
	/// </summary>
	/// <param name="id">User id</param>
	/// <returns>User data</returns>
	/// <exception cref="UserNotFoundException">User with specified id not found</exception>
	Task<UserDto> Get(string id);
	
	/// <summary>
	/// Update user data
	/// </summary>
	/// <param name="id">User id</param>
	/// <param name="dto">New user data</param>
	/// <returns>Updated user data</returns>
	/// <exception cref="UserNotFoundException">User with specified id not found</exception>
	Task<UserDto> Update(string id, UserUpdateDto dto);
	
	/// <summary>
	/// Delete user
	/// </summary>
	/// <param name="id">User id</param>
	/// <returns>Flag specifying whether the user was deleted successfully</returns>
	/// <exception cref="UserNotFoundException">User with specified id not found</exception>
	Task<bool> Delete(string id);
}