using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class adddefaultrole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "321fda66-15ad-4239-a90f-2238929bc99a", "d0c47168-881a-4eaf-a4c2-20c8860a9c9d", "TRAINER", "trainer" },
                    { "cb057132-f2b5-4304-89ad-181c91fb5853", "47abada2-0012-4034-bafe-96dacbb95a5f", "ADMIN", "admin" },
                    { "d440fb55-34db-4d23-b204-dc07355bd496", "513a9014-f562-42fd-8104-25a8be6d129f", "TRAINEE", "trainee" },
                    { "d54aaf3b-88cc-4e08-b884-08135141e550", "6e3b6ece-68f4-4e54-9629-6a958d83aec2", "STAFF", "staff" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "321fda66-15ad-4239-a90f-2238929bc99a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb057132-f2b5-4304-89ad-181c91fb5853");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d440fb55-34db-4d23-b204-dc07355bd496");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d54aaf3b-88cc-4e08-b884-08135141e550");
        }
    }
}
