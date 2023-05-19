using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class CredentialException : InvalidArgumentException
{
	public CredentialException() : base("Неверный логин или пароль")
	{
	}
}