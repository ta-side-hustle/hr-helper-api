using System;

namespace Application.Auth.Dto
{
	public class TokenDto
	{
		public string AccessToken { get; set; }
		public DateTime Expires { get; set; }
	}
}