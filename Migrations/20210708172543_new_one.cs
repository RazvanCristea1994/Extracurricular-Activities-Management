using Microsoft.EntityFrameworkCore.Migrations;

namespace ExtracurricularActivitiesManagement.Migrations
{
    public partial class new_one : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38c621b1-c549-4ca3-ab20-7d5258fd159f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "876bbaa2-9245-43b2-b7f1-9115850765f9");

            migrationBuilder.DropColumn(
                name: "ActivityType",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "b1e33755-abc2-4367-a5ac-cd1eeb71cd7b", "857d8930-d2ce-4bbd-a515-b5ce7c164ba3", "UserRole", "AppStudent", "APPSTUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "56121e52-757d-4928-8659-e1c789c5e523", "8ab3b62f-5175-4b28-9145-4e1f2a5d7ec8", "UserRole", "AppAdmin", "APPADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56121e52-757d-4928-8659-e1c789c5e523");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1e33755-abc2-4367-a5ac-cd1eeb71cd7b");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Activities");

            migrationBuilder.AddColumn<int>(
                name: "ActivityType",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "38c621b1-c549-4ca3-ab20-7d5258fd159f", "c7fa36b6-29b6-4902-9849-73e20f1699b5", "UserRole", "AppStudent", "APPSTUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "876bbaa2-9245-43b2-b7f1-9115850765f9", "8936b2bb-2ac8-4933-89c8-3b4531139823", "UserRole", "AppAdmin", "APPADMIN" });
        }
    }
}
