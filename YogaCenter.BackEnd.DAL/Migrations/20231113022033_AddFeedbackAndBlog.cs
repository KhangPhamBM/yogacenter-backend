using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class AddFeedbackAndBlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29192ce8-caa4-4c2a-95f9-90cee75b50f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7eef2b64-2027-4f18-8c94-3a1562112ca7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "990f4ae2-8172-4c83-bfb3-03be9b392876");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a361f267-ae16-40f9-9e0c-5a51737b723b");

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogImg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassDetailId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_ClassDetails_ClassDetailId",
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
                    { "0bbe6059-e13c-4e15-ad5c-3f7388eb5e81", "cf54c2c1-9758-4678-8ab2-bd98c11801eb", "STAFF", "staff" },
                    { "191cb938-5ee0-49ec-b558-bff7e7695d5c", "368c13ac-3509-4ed7-8319-d37626241f09", "ADMIN", "admin" },
                    { "2d5ee189-e68b-4a8e-8cea-f30c4ac2a132", "4dd0aad9-ac48-478e-ac57-f5da9e2b7a0d", "TRAINER", "trainer" },
                    { "48e83a69-eb6b-4740-90f5-823451dd849e", "6008ca75-661a-4207-9fe6-0a070e21e9bb", "TRAINEE", "trainee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ClassDetailId",
                table: "Feedbacks",
                column: "ClassDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bbe6059-e13c-4e15-ad5c-3f7388eb5e81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "191cb938-5ee0-49ec-b558-bff7e7695d5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d5ee189-e68b-4a8e-8cea-f30c4ac2a132");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48e83a69-eb6b-4740-90f5-823451dd849e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29192ce8-caa4-4c2a-95f9-90cee75b50f8", "dceaa390-c82a-4e07-9df7-5610be1a2aed", "ADMIN", "admin" },
                    { "7eef2b64-2027-4f18-8c94-3a1562112ca7", "2f34f12c-6158-482d-9fc9-b2a11213a2e0", "TRAINER", "trainer" },
                    { "990f4ae2-8172-4c83-bfb3-03be9b392876", "9b5e53d3-0a50-4995-aa81-40d5558eed4e", "TRAINEE", "trainee" },
                    { "a361f267-ae16-40f9-9e0c-5a51737b723b", "2536e9cb-6e08-46b0-9162-64656d31470a", "STAFF", "staff" }
                });
        }
    }
}
