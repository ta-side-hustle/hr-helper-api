using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Auth.Dto;
using Application.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Models.Auth;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Throw;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
	private readonly JwtOptions _jwOptions;
	private readonly UserManager<UserModel> _userManager;

	public AuthService(IOptions<JwtOptions> jwOptions, UserManager<UserModel> userManager)
	{
		_userManager = userManager;
		_jwOptions = jwOptions.Value;
	}

	public async Task<TokenDto> Authenticate(CredentialDto dto)
	{
		var user = await _userManager.FindByEmailAsync(dto.Email);

		user.ThrowIfNull(() => new CredentialException());

		(await IsCredentialValid(user, dto.Password)).Throw(() => throw new CredentialException()).IfFalse();

		return GetToken(user);
	}

	private async Task<bool> IsCredentialValid(UserModel user, string password)
	{
		return await _userManager.CheckPasswordAsync(user, password);
	}

	private IEnumerable<Claim> GetClaims(UserModel user)
	{
		return new List<Claim>
		{
			new(ClaimTypes.Name, user.Email),
			new(ClaimTypes.NameIdentifier, user.Id),
			new(JwtRegisteredClaimNames.Aud, _jwOptions.Audience),
			new(JwtRegisteredClaimNames.Iss, _jwOptions.Issuer),
			new(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
			new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(_jwOptions.Expires).ToUnixTimeSeconds().ToString())
		};
	}

	private TokenDto GetToken(UserModel user)
	{
		var signingCredentials =
			new SigningCredentials(_jwOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
		var claims = GetClaims(user);

		var tokenHeader = new JwtHeader(signingCredentials);
		var tokenPayload = new JwtPayload(claims);

		var token = new JwtSecurityToken(tokenHeader, tokenPayload);
		var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

		return new TokenDto
		{
			AccessToken = tokenStr,
			Expires = _jwOptions.Expires
		};
	}
}