using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class addMessageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassDetailId = table.Column<int>(type: "int", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_ClassDetails_ClassDetailId",
                        column: x => x.ClassDetailId,
                        principalTable: "ClassDetails",
                        principalColumn: "ClassDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a66e9c3-ae2f-4f2f-8ea3-b4fe8fb703dc", "4f3d3252-e188-470c-b320-3224337b5aa0", "TRAINEE", "trainee" },
                    { "69c73da1-9a01-40f9-b9d7-ae2e126108d1", "89cdd34a-17f1-4d9f-855f-0befe322e6ed", "STAFF", "staff" },
                    { "76f43a29-5e58-40ec-b8b8-7a71a10b7c45", "b94b4bf2-2c6e-4b1a-97f0-ebdb23c8c48f", "TRAINER", "trainer" },
                    { "7fd8d8ed-f289-469e-8b18-f899a84de0bd", "5a10837e-46fa-4042-af71-3b5017d293f5", "ADMIN", "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClassDetailId",
                table: "Messages",
                column: "ClassDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a66e9c3-ae2f-4f2f-8ea3-b4fe8fb703dc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c73da1-9a01-40f9-b9d7-ae2e126108d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76f43a29-5e58-40ec-b8b8-7a71a10b7c45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7fd8d8ed-f289-469e-8b18-f899a84de0bd");

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
    }
}
