#nullable enable
using System.Collections.Generic;

namespace Domain.Exceptions.Base;

public abstract class ListException : BaseException
{
	public IEnumerable<string> Messages { get; set; }

	protected ListException(string? message, IEnumerable<string> messages):base(message)
	{
		Messages = messages;
	}
}