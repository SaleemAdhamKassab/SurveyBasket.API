using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Helpers;
using SurveyBasket.API.Models;
using SurveyBasket.API.Models.Data;

namespace SurveyBasket.API.Services.PollsNotificationService
{
	public class PollsNotificationService(
		ApplicationDbContext db,
		UserManager<ApplicationUser> userManager,
		IHttpContextAccessor httpContextAccessor,
		IEmailSender emailSender) : IPollsNotificationService
	{
		private readonly ApplicationDbContext _db = db;
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		private readonly IEmailSender _emailSender = emailSender;


		public async Task SendNewPollsNotification()
		{
			var polls = await _db.Polls
				.Where(e => e.IsPublished && e.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
				.AsNoTracking()
				.ToListAsync();

			// TODO: select members only - after Role lesson
			var users = await _userManager.Users.ToListAsync();

			var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

			foreach (var poll in polls)
			{
				foreach (var user in users)
				{
					var pollNotificationTemplateModel = new Dictionary<string, string>()
					{
						{
							"{{name}}",user.FirstName
						},
						{
							"{{pollTitle}}",poll.Title
						},
						{
							"{{endDate}}",poll.EndsAt.ToString()
						},
						{
							"{{url}}",$"{origin}/polls/start/{poll.Id}"
						}
					};

					var emailBody = EmailBodyBuilder.GenerateEmailBody("PollNotification", pollNotificationTemplateModel);

					await _emailSender.SendEmailAsync(user.Email,
													 $"Survey Basket: New Poll - {poll.Title}",
													 emailBody);
				}
			}
		}
	}
}
