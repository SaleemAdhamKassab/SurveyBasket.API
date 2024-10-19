﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.API.Contracts.Auth;
using SurveyBasket.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.API.Services.Auth
{
	public interface IJwtProvider
	{
		(string token, int expiresIn) GenerationToken(ApplicationUser user);
		string? ValidateToken(string token); // return UserId if valid
	}

	public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
	{
		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		public (string token, int expiresIn) GenerationToken(ApplicationUser user)
		{
			Claim[] claims = [
				new (JwtRegisteredClaimNames.Sub,user.Id),
				new (JwtRegisteredClaimNames.Email,user.Email!),
				new (JwtRegisteredClaimNames.GivenName,user.FirstName),
				new (JwtRegisteredClaimNames.FamilyName,user.LastName),
				new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
			];

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var expiresIn = 30;
			var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);

			var token = new JwtSecurityToken(
				issuer: _jwtOptions.Issuer,
				audience: _jwtOptions.Audience,
				claims: claims,
				expires: expirationDate,
				signingCredentials: signingCredentials
			);

			return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn);
		}

		public string? ValidateToken(string token)
		{
			JwtSecurityTokenHandler tokenHandler = new();
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters()
				{
					IssuerSigningKey = symmetricSecurityKey,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				string userId = jwtToken.Claims.First(e => e.Type == JwtRegisteredClaimNames.Sub).Value;
				return userId;
			}
			catch
			{
				return null;
			}
		}
	}
}
