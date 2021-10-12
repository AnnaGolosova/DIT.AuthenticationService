using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationService.Infrastructure.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c65a4408-2a9e-4cd3-95e0-eb968cc2f170", "0278d0a0-31f6-476f-8826-2acbde86c4d3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9b2ff458-1180-4ba9-9b9a-d346c4ea563e", "6ca116d1-09fb-49b0-a481-63fef1eb04ef", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8219240d-61f9-4139-9d4b-fccff1eba7f2", "19302952-038a-4a54-ad36-a406185fa728", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8219240d-61f9-4139-9d4b-fccff1eba7f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b2ff458-1180-4ba9-9b9a-d346c4ea563e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c65a4408-2a9e-4cd3-95e0-eb968cc2f170");
        }
    }
}
