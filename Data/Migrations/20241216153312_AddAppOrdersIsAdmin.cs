using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stronghold.AppDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppOrdersIsAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IntegratedAppOrders",
                table: "IntegratedAppOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppDirectoryOrders",
                table: "AppDirectoryOrders");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "IntegratedAppOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AppDirectoryOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IntegratedAppOrders",
                table: "IntegratedAppOrders",
                columns: new[] { "UserId", "IsAdmin", "IntegratedAppEntryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppDirectoryOrders",
                table: "AppDirectoryOrders",
                columns: new[] { "UserId", "IsAdmin", "AppDirectoryEntryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IntegratedAppOrders",
                table: "IntegratedAppOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppDirectoryOrders",
                table: "AppDirectoryOrders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IntegratedAppOrders",
                table: "IntegratedAppOrders",
                columns: new[] { "UserId", "IntegratedAppEntryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppDirectoryOrders",
                table: "AppDirectoryOrders",
                columns: new[] { "UserId", "AppDirectoryEntryId" });
        }
    }
}
