using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class OrganizationNotFoundException: NullException
{
	public OrganizationNotFoundException() : base("Организация не найдена")
	{
	}
}