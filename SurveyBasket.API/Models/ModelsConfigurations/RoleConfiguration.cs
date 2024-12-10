using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Models.ModelsConfigurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.HasData([
				new ApplicationRole{
					Id=  DefaultRoles.AdminRoleId,
					Name = DefaultRoles.Admin,
					NormalizedName =  DefaultRoles.Admin.ToUpper(),
					ConcurrencyStamp = DefaultRoles.AdminRoleConcurrencyStamp,
					IsDeleted = false,
					IsDefault= false,
				},
				new ApplicationRole{
					Id=  DefaultRoles.MemberRoleId,
					Name = DefaultRoles.Member,
					NormalizedName =  DefaultRoles.Member.ToUpper(),
					ConcurrencyStamp = DefaultRoles.MemberRoleConcurrencyStamp,
					IsDeleted = false,
					IsDefault= true,
				}
			]);
		}
	}
}
