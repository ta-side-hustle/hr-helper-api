using System.Threading.Tasks;
using Application.Auth.Dto;
using Domain.Exceptions;

namespace Application.Auth.Interfaces
{
	public interface IAuthService
	{
		/// <summary>
		/// Authenticates user credential
		/// </summary>
		/// <param name="dto">User credential</param>
		/// <returns>Access token</returns>
		/// <exception cref="CredentialException">User not found or credential is invalid</exception>
		Task<TokenDto> Authenticate(CredentialDto dto);
	}
}