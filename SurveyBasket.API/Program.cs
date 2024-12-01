using Hangfire;
using HangfireBasicAuthenticationFilter;
using Serilog;
using SurveyBasket.API;

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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();

app.Run();