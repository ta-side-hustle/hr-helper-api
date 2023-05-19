#nullable enable
using System.Security.Claims;
using Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Base.Controller;

[Authorize]
[ApiController]
[AppExceptionFilter]
[Route("[controller]")]
public class AppControllerBase : ControllerBase
{
	protected string? UserId => HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
}