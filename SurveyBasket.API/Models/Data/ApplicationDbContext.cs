using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SurveyBasket.API.Extensions;
using System.Reflection;

namespace SurveyBasket.API.Models.Data
{
	public class ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public DbSet<Poll> Polls { get; set; } = default!;
		public DbSet<Question> Questions { get; set; } = default!;
		public DbSet<Answer> Answers { get; set; } = default!;
		public DbSet<Vote> Votes { get; set; } = default!;
		public DbSet<VoteAnswer> VoteAnswers { get; set; } = default!;
		public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			var cascadeFKs = modelBuilder.Model
				.GetEntityTypes()
				.SelectMany(t => t.GetForeignKeys().Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership));

			foreach (var fk in cascadeFKs)
				fk.DeleteBehavior = DeleteBehavior.Restrict;


			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditModel>();
			string currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId()!;

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


		// To Handle the error: PendingModelChangesWarning Not Recommended
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.ConfigureWarnings(warnings =>
				warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
		}
	}
}
