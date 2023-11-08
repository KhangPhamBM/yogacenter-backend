using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class editfieldnameofclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxOfTrainer",
                table: "Classes",
                newName: "MaxOfTrainee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxOfTrainee",
                table: "Classes",
                newName: "MaxOfTrainer");
        }
    }
}
