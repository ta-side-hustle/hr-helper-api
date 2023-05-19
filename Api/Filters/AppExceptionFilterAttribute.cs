using System;
using System.Net;
using Api.Modules.Base.ViewModel;
using Domain.Exceptions.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class AppExceptionFilterAttribute : Attribute, IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		var exception = context.Exception;

		if (exception is not BaseException appException) return;

		context.Result = GetResponseObject(appException);
		context.ExceptionHandled = true;
	}

	private static ObjectResult GetResponseObject(BaseException exception)
	{
		return exception switch
		{
			ListException listException => new ErrorResult(listException.Messages).ToResult(HttpStatusCode.BadRequest),

			InvalidArgumentException invalidArgumentException => new ErrorResult(invalidArgumentException.Message)
				.ToResult(HttpStatusCode.BadRequest),

			NullException nullException => new ErrorResult(nullException.Message).ToResult(HttpStatusCode.NotFound),

			_ => new ErrorResult(exception.Message).ToResult(HttpStatusCode.BadRequest)
		};
	}
}