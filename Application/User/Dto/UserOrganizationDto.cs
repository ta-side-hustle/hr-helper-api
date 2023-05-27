using System.Collections.Generic;
using Application.Organization.Dto;

namespace Application.User.Dto;

public class UserOrganizationDto
{
	public OrganizationDto Organization { get; set; }
	public IEnumerable<UserRoleDto> Roles { get; set; }
}