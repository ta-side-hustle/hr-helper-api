namespace Domain.Exceptions.Base;

public abstract class InvalidArgumentException: BaseException
{
	protected InvalidArgumentException(string message) : base(message)
	{
	}
}