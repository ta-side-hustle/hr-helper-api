using Domain.Models.Auth;

namespace Domain.Models.Organization;

public class UserOrganizationModel
{
	public string UserId { get; set; }
	public int OrganizationId { get; set; }
	public string RoleId { get; set; }

	public UserModel User { get; set; }
	public OrganizationModel Organization { get; set; }
	public RoleModel Role { get; set; }
}