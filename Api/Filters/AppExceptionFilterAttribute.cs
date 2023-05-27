using System;
using System.Net;
using Api.Modules.Base.ViewModel;
using Domain.Exceptions.Base;
using Infrastructure.Authorization.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

[AttributeUsage(AttributeTargets.Class)]
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
			AuthorizationException authorizationException => new ErrorResult(authorizationException.Messages).ToResult(
				HttpStatusCode.Forbidden),

			ListException listException => new ErrorResult(listException.Messages).ToResult(HttpStatusCode.BadRequest),

			InvalidArgumentException invalidArgumentException => new ErrorResult(invalidArgumentException.Message)
				.ToResult(HttpStatusCode.BadRequest),

			NullException nullException => new ErrorResult(nullException.Message).ToResult(HttpStatusCode.NotFound),


			_ => new ErrorResult(exception.Message).ToResult(HttpStatusCode.BadRequest)
		};
	}
}