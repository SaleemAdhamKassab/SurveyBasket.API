namespace SurveyBasket.API.Settings
{
	public class MailOptions
	{
		public string Email { get; set; } = string.Empty;
		public string DisplayName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Host { get; set; } = string.Empty;
		public int Port { get; set; }
	}
}
