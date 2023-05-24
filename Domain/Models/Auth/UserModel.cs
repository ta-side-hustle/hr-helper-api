using System.Collections.Generic;
using Domain.Models.Organization;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Auth
{
	public class UserModel: IdentityUser
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		
		public ICollection<UserOrganizationModel> Organizations { get; set; }
	}
}