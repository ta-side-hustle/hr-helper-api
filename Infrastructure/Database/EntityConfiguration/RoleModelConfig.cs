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
		
		#region Default roles seeding

		builder.HasData(
			new RoleModel
			{
				Id = "5b8e7d97-2fef-473f-874d-1ae5bc7c3bcf",
				ConcurrencyStamp = "cd448e2d-e19e-4e65-bdad-8df8fd135d72",
				Name = "owner", NormalizedName = "OWNER",
				LocalizedName = "Владелец", Description = "Имеет полный доступ к управлению организацией"
			},
			new RoleModel
			{
				Id = "9e412baa-7d46-4fe4-8f3f-6c69c6fd9ce8",
				ConcurrencyStamp = "f3f87ce8-1b2e-4300-9c7a-f04d24a99638",
				Name = "admin", NormalizedName = "ADMIN",
				LocalizedName = "Администратор", Description = "Управление сотрудниками и редактирование данных организации"
			},
			new RoleModel
			{
				Id = "bd61de7f-6110-4356-a4d7-bd444936cf9c",
				ConcurrencyStamp = "86c7424c-47e4-463a-8cc5-4fbcab3dd130",
				Name = "employee", NormalizedName = "EMPLOYEE",
				LocalizedName = "Сотрудник", Description = "Сотрудник HR-отдела"
			}
		);

		#endregion
	}
}