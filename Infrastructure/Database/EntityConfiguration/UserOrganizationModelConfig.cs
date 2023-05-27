using Domain.Models.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class UserOrganizationModelConfig : IEntityTypeConfiguration<UserOrganizationModel>
{
	public void Configure(EntityTypeBuilder<UserOrganizationModel> builder)
	{
		builder
			.ToTable("UserOrganizations")
			.HasKey(x => new { x.UserId, x.OrganizationId, x.RoleId});

		builder
			.HasOne(x => x.User)
			.WithMany(x => x.Organizations)
			.HasForeignKey(x => x.UserId);
		
		builder
			.HasOne(x => x.Role)
			.WithMany(x => x.Organizations)
			.HasForeignKey(x => x.RoleId);
		
		builder
			.HasOne(x => x.Organization)
			.WithMany(x => x.Users)
			.HasForeignKey(x => x.OrganizationId);
	}
}