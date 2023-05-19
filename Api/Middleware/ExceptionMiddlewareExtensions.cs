using System.Net;
using Api.Modules.Base.ViewModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware;

public static class ExceptionMiddlewareExtensions
{
	public static void ConfigureExceptionHandler(this IApplicationBuilder app)
	{
		app.UseExceptionHandler(appError =>
		{
			appError.Run(async context =>
			{
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";
				
				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

				if (contextFeature != null)
				{
					var exception = contextFeature.Error;

					await context.Response.WriteAsync(new ErrorResult(exception.Message).ToString());
				}
			});
		});
	}
}