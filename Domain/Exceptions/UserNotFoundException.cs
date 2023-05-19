using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class UserNotFoundException : NullException
{
	public UserNotFoundException() : base("Пользователь не найден")
	{
	}
}