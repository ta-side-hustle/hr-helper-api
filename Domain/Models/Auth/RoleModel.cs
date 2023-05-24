using System.Collections.Generic;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Auth;

public class RoleModel : IdentityRole
{
	/// <summary>
	/// Localized role name
	/// </summary>
	public string LocalizedName { get; set; }
	
	/// <summary>
	/// Short description of the role
	/// </summary>
	public string Description { get; set; }
	
	public ICollection<UserOrganizationModel> Organizations { get; set; }
}