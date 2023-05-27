using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Api.Modules.Base.Controller;
using Api.Modules.Base.ViewModel;
using Application.Organization.Dto;
using Application.Organization.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Modules.Organization.Controller;

/// <summary>Organization management</summary>
public class OrganizationController : AppControllerBase
{
	private readonly IOrganizationService _organizationService;

	public OrganizationController(IOrganizationService organizationService)
	{
		_organizationService = organizationService;
	}

	/// <summary>
	/// [auth] List of user organizations
	/// </summary>
	[HttpGet("")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<IList<OrganizationDto>>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> GetUserOrganizations()
	{
		var organization = await _organizationService.GetAllByUserAsync(UserId);
		return new SuccessResult<IList<OrganizationDto>>(organization).ToResult(HttpStatusCode.OK);
	}

	/// <summary>
	/// [auth, employee] Organization info
	/// </summary>
	[HttpGet("{id:int}")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<IList<OrganizationDto>>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.Forbidden, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> GetOrganization([FromRoute] int id)
	{
		var organization = await _organizationService.GetAsync(id);
		return new SuccessResult<OrganizationDto>(organization).ToResult(HttpStatusCode.OK);
	}
	
	/// <summary>
	/// [auth] New organization
	/// </summary>
	[HttpPost("")]
	[SwaggerResponse((int)HttpStatusCode.Created, type: typeof(ResourceCreateResult<int>))]
	public async Task<IActionResult> CreateOrganization([FromBody] OrganizationCreateDto dto)
	{
		var result = await _organizationService.CreateAsync(dto, UserId);
		return new ResourceCreateResult<int>(result).ToResult(HttpStatusCode.Created);
	}
	
	/// <summary>
	/// [auth, admin] Update existing organization
	/// </summary>
	[HttpPut("")]
	[SwaggerResponse((int)HttpStatusCode.OK, type: typeof(SuccessResult<OrganizationDto>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.Forbidden, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> UpdateOrganization([FromBody] OrganizationDto dto)
	{
		var result = await _organizationService.UpdateAsync(dto);
		return new SuccessResult<OrganizationDto>(result).ToResult(HttpStatusCode.OK);
	}
	
	/// <summary>
	/// [auth, owner] Delete organization
	/// </summary>
	[HttpDelete("{id:int}")]
	[SwaggerResponse((int)HttpStatusCode.NoContent, type: typeof(SuccessResult<OrganizationDto>))]
	[SwaggerResponse((int)HttpStatusCode.Unauthorized, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.Forbidden, type: typeof(ErrorResult))]
	[SwaggerResponse((int)HttpStatusCode.NotFound, type: typeof(ErrorResult))]
	public async Task<IActionResult> DeleteOrganization([FromRoute] int id)
	{
		await _organizationService.DeleteAsync(id);
		return NoContent();
	}
}