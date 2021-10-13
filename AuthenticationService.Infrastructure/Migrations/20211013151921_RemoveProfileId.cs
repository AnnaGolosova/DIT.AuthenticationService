using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationService.Infrastructure.Migrations
{
    public partial class RemoveProfileId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "76a3f858-984a-4947-ac8d-4441a4db85f7", "db4f2d89-591b-4c1a-8e56-9735359a6aa0", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88000426-136e-4895-bfd1-f5aa62de958d", "5cc903b4-cb42-427c-a4a4-854d97295d02", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "74ed05d6-3e8b-4b4e-9a8f-30d020546e2c", "5e8cc349-d49f-488b-813d-9113268a87f5", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74ed05d6-3e8b-4b4e-9a8f-30d020546e2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76a3f858-984a-4947-ac8d-4441a4db85f7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88000426-136e-4895-bfd1-f5aa62de958d");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
