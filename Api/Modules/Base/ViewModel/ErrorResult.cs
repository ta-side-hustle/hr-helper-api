#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Api.Modules.Base.ViewModel;

public class ErrorResult : Result<object>
{
	public ErrorResult(IEnumerable<string> errors)
	{
		IsError = true;
		Data = default;
		Errors = errors.ToList();
	}

	public ErrorResult(string error)
	{
		IsError = true;
		Data = default;
		Errors = new List<string>
		{
			error
		};
	}
}