namespace Domain.Exceptions.Base;

public abstract class NullException : BaseException
{
	protected NullException(string message) : base(message)
	{
	}
}