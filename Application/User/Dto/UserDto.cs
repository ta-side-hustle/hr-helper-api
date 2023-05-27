using System.Collections.Generic;

namespace Application.User.Dto;

public class UserDto
{
	public string LastName { get; set; }
	public string FirstName { get; set; }
	public string Email { get; set; }

	public IEnumerable<UserOrganizationDto> Organizations { get; set; }
}