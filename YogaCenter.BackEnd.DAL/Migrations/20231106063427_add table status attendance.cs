using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class addtablestatusattendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAttended",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceStatusId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttendanceStatuses",
                columns: table => new
                {
                    AttendanceStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceStatuses", x => x.AttendanceStatusId);
                });

            migrationBuilder.InsertData(
                table: "AttendanceStatuses",
                columns: new[] { "AttendanceStatusId", "AttendanceStatusName" },
                values: new object[] { 1, "NOT YET" });

            migrationBuilder.InsertData(
                table: "AttendanceStatuses",
                columns: new[] { "AttendanceStatusId", "AttendanceStatusName" },
                values: new object[] { 2, "Attended" });

            migrationBuilder.InsertData(
                table: "AttendanceStatuses",
                columns: new[] { "AttendanceStatusId", "AttendanceStatusName" },
                values: new object[] { 3, "Absent" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AttendanceStatusId",
                table: "Attendances",
                column: "AttendanceStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AttendanceStatuses_AttendanceStatusId",
                table: "Attendances",
                column: "AttendanceStatusId",
                principalTable: "AttendanceStatuses",
                principalColumn: "AttendanceStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AttendanceStatuses_AttendanceStatusId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "AttendanceStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_AttendanceStatusId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AttendanceStatusId",
                table: "Attendances");

            migrationBuilder.AddColumn<bool>(
                name: "isAttended",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
