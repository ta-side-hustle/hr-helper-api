using Domain.Models.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class OrganizationModelConfig : IEntityTypeConfiguration<OrganizationModel>
{
	public void Configure(EntityTypeBuilder<OrganizationModel> builder)
	{
		builder
			.ToTable("Organizations")
			.HasKey(x => x.Id);
		
		builder.Property(x => x.Id)
			.UseIdentityColumn();

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(64)
			.HasColumnType("nvarchar(64)");
	}
}