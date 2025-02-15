using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SurveyBasket.API;
using SurveyBasket.API.Services.PollsNotificationService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependecies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard("/HangfireJobs", new DashboardOptions
{
	Authorization = [
		new HangfireCustomBasicAuthenticationFilter{
			User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
			Pass=app.Configuration.GetValue<string>("HangfireSettings:Password")
		}
	]
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var pollsNotificationService = scope.ServiceProvider.GetRequiredService<IPollsNotificationService>();

RecurringJob.AddOrUpdate("SendNewPollsNotification",
						 () => pollsNotificationService.SendNewPollsNotification(),
						 Cron.Daily
);

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();
app.MapHealthChecks("health", new HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseRateLimiter();

app.Run();