using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Auth.Interfaces;

public interface IAuthorizationGuard
{
	/// <summary>
	///		Authorize access to requested resource for <see cref="ClaimsPrincipal"/> of the request.
	/// </summary>
	/// <param name="resource">The resource to which access is requested.</param>
	/// <param name="policy">The name of the policy that decides to allow or deny access.</param>
	/// <returns>The <see cref="Task" /> that represents the asynchronous operation.</returns>
	/// <exception cref="AuthorizationException">Access for requested resource is denied.</exception>
	/// <exception cref="AuthenticationException">User is not authenticated.</exception>
	Task AuthorizeAsync(object resource, string policy);
}