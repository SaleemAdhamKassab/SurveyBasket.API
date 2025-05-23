﻿using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Settings
{
	public class JwtOptions
	{
		public static string SectionName = "Jwt";
		[Required]
		public string Key { get; set; } = string.Empty;
		[Required]
		public string Issuer { get; set; } = string.Empty;
		[Required]
		public string Audience { get; set; } = string.Empty;
		[Range(1, 1000)]
		public int ExpiryMinutes { get; set; }
	}
}
