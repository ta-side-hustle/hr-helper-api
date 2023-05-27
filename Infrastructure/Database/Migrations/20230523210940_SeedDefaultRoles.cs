using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "LocalizedName", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b8e7d97-2fef-473f-874d-1ae5bc7c3bcf", "cd448e2d-e19e-4e65-bdad-8df8fd135d72", "Имеет полный доступ к управлению организацией", "RoleModel", "Владелец", "owner", "OWNER" },
                    { "9e412baa-7d46-4fe4-8f3f-6c69c6fd9ce8", "f3f87ce8-1b2e-4300-9c7a-f04d24a99638", "Управление сотрудниками и редактирование данных организации", "RoleModel", "Администратор", "admin", "ADMIN" },
                    { "bd61de7f-6110-4356-a4d7-bd444936cf9c", "86c7424c-47e4-463a-8cc5-4fbcab3dd130", "Сотрудник HR-отдела", "RoleModel", "Сотрудник", "employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b8e7d97-2fef-473f-874d-1ae5bc7c3bcf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e412baa-7d46-4fe4-8f3f-6c69c6fd9ce8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd61de7f-6110-4356-a4d7-bd444936cf9c");
        }
    }
}
