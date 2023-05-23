using Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class RoleModelConfig : IEntityTypeConfiguration<RoleModel>
{
	public void Configure(EntityTypeBuilder<RoleModel> builder)
	{
		builder.Property(user => user.LocalizedName)
			.IsRequired()
			.HasMaxLength(256)
			.HasColumnType("nvarchar(256)");

		builder.Property(user => user.Description)
			.IsRequired()
			.HasMaxLength(450)
			.HasColumnType("nvarchar(450)");
	}
}