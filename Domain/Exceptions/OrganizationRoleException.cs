using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class OrganizationRoleException : InvalidArgumentException
{
	public OrganizationRoleException(string message) : base(message)
	{
	}
}