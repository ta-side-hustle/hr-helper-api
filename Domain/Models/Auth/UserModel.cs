using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Auth
{
	public class UserModel: IdentityUser
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
	}
}