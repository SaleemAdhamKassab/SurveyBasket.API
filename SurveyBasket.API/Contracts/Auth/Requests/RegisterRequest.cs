﻿namespace SurveyBasket.API.Contracts.Auth.Requests
{
	public class RegisterRequest
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty; // Email is UserName also
		public string Password { get; set; } = string.Empty;
	}
}