using Domain.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.EntityConfiguration;

public class UserModelConfig : IEntityTypeConfiguration<UserModel>
{
	public void Configure(EntityTypeBuilder<UserModel> builder)
	{
		builder.Property(user => user.LastName)
			.IsRequired()
			.HasMaxLength(20)
			.HasColumnType("nvarchar(20)");

		builder.Property(user => user.FirstName)
			.IsRequired()
			.HasMaxLength(20)
			.HasColumnType("nvarchar(20)");
	}
}