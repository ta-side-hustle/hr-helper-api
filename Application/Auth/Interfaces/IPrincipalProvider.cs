#nullable enable
using System.Security.Principal;

namespace Application.Auth.Interfaces;

public interface IPrincipalProvider
{
	/// <summary>
	/// Represents an identity of the user on whose behalf the request is made. 
	/// </summary>
	IPrincipal? Principal { get; }
}