using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stronghold.AppDashboard.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAppOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnPremSid",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AppDirectoryOrders",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AppDirectoryEntryId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDirectoryOrders", x => new { x.UserId, x.AppDirectoryEntryId });
                    table.ForeignKey(
                        name: "FK_AppDirectoryOrders_IntegratedApps_AppDirectoryEntryId",
                        column: x => x.AppDirectoryEntryId,
                        principalTable: "IntegratedApps",
                        principalColumn: "IntegratedAppEntryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppDirectoryOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegratedAppOrders",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IntegratedAppEntryId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegratedAppOrders", x => new { x.UserId, x.IntegratedAppEntryId });
                    table.ForeignKey(
                        name: "FK_IntegratedAppOrders_IntegratedApps_IntegratedAppEntryId",
                        column: x => x.IntegratedAppEntryId,
                        principalTable: "IntegratedApps",
                        principalColumn: "IntegratedAppEntryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegratedAppOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDirectoryOrders_AppDirectoryEntryId",
                table: "AppDirectoryOrders",
                column: "AppDirectoryEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegratedAppOrders_IntegratedAppEntryId",
                table: "IntegratedAppOrders",
                column: "IntegratedAppEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDirectoryOrders");

            migrationBuilder.DropTable(
                name: "IntegratedAppOrders");

            migrationBuilder.AddColumn<string>(
                name: "OnPremSid",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
