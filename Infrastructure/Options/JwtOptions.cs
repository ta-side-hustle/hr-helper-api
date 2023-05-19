using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Options
{
	public class JwtOptions
	{
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Key { get; set; }
		public int Lifetime { get; set; }
		
		public DateTime Expires => DateTime.Now.AddSeconds(Lifetime);
		public SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(Key));
	}
}