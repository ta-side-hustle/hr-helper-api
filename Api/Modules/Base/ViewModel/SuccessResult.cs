#nullable enable
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Base.ViewModel;

public class SuccessResult<T> : Result<T>
{
	public SuccessResult(T data)
	{
		IsError = false;
		Data = data;
		Errors = default;
	}
	
	public new ObjectResult ToResult(HttpStatusCode statusCode)
	{
		return new ObjectResult(this)
		{
			StatusCode = (int)statusCode
		};
	} 
}