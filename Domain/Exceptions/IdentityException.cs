using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Exceptions;

public class IdentityException : ListException
{
	public IdentityException(IEnumerable<IdentityError> identityErrors)
		: base("При сохранении данных произошла непредвиденная ошибка", identityErrors.Select(e => e.Description))
	{
	}
}