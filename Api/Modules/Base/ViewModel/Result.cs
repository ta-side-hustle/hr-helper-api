#nullable enable
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Base.ViewModel;

public abstract class Result<T>
{
	public bool IsError { get; set; }
	public IList<string>? Errors { get; set; }
	public T? Data { get; set; }

	public ObjectResult ToResult(HttpStatusCode statusCode)
	{
		return new ObjectResult(this)
		{
			StatusCode = (int)statusCode
		};
	}

	public override string ToString()
	{
		return JsonSerializer.Serialize(this);
	}

	public string ToString(JsonSerializerOptions? options)
	{
		return JsonSerializer.Serialize(this, options);
	}
}