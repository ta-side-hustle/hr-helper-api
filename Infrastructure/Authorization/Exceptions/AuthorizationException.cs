using System.Linq;
using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Exceptions;

public class AuthorizationException : ListException
{
	public AuthorizationException(AuthorizationFailure failure) : base("Недостаточно прав",
		failure.FailureReasons.Select(reason => reason.Message))
	{
	}
}