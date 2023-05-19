using System;

namespace Api.Modules.User.ViewModel;

public class LoginResponse
{
	public string AccessToken { get; set; }
	public DateTime Expires { get; set; }
}