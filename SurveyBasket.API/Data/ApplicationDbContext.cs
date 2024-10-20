using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Models;
using System.Reflection;
using System.Security.Claims;

namespace SurveyBasket.API.Data
{
	public class ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public DbSet<Poll> Polls { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditModel>();
			string currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

			foreach (var entityEntry in entries)
			{
				if (entityEntry.State == EntityState.Added)
					entityEntry.Property(e => e.CreatedById).CurrentValue = currentUserId;
				else if (entityEntry.State == EntityState.Modified)
				{
					entityEntry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
					entityEntry.Property(e => e.UpdateOn).CurrentValue = DateTime.UtcNow;
				}
			}

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
	}
}
