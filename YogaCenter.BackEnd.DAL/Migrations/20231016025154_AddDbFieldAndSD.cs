using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YogaCenter.BackEnd.DAL.Migrations
{
    public partial class AddDbFieldAndSD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxOfTrainer",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinOfTrainee",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "PaymentTypeId", "Type" },
                values: new object[,]
                {
                    { 1, "VNPAY" },
                    { 2, "MOMO" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "RoomName" },
                values: new object[,]
                {
                    { 1, "A01" },
                    { 2, "A02" },
                    { 3, "A03" },
                    { 4, "A04" },
                    { 5, "A05" },
                    { 6, "A06" },
                    { 7, "A07" },
                    { 8, "A08" },
                    { 9, "A09" },
                    { 10, "A10" },
                    { 11, "B01" },
                    { 12, "B02" },
                    { 13, "B03" },
                    { 14, "B04" },
                    { 15, "B05" },
                    { 16, "B06" },
                    { 17, "B07" },
                    { 18, "B08" },
                    { 19, "B09" },
                    { 20, "B10" }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionStatuses",
                columns: new[] { "SubscriptionStatusId", "SubscriptionStatusName" },
                values: new object[,]
                {
                    { 1, "Successful" },
                    { 2, "Failed" },
                    { 3, "Pending" }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "TicketStatusId", "TicketStatusName" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "TicketTypeId", "TicketName" },
                values: new object[,]
                {
                    { 1, "Refund ticket" },
                    { 2, "Other ticket" }
                });

            migrationBuilder.InsertData(
                table: "TimeFrames",
                columns: new[] { "TimeFrameId", "TimeFrameName" },
                values: new object[,]
                {
                    { 1, "7H00 - 9H00" },
                    { 2, "9H00 - 11H00" },
                    { 3, "13H00 - 15H00" },
                    { 4, "17H00 - 19H00" },
                    { 5, "19H00 - 21H00" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "PaymentTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "PaymentTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SubscriptionStatuses",
                keyColumn: "SubscriptionStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubscriptionStatuses",
                keyColumn: "SubscriptionStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubscriptionStatuses",
                keyColumn: "SubscriptionStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TicketStatuses",
                keyColumn: "TicketStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketStatuses",
                keyColumn: "TicketStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TicketStatuses",
                keyColumn: "TicketStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketTypes",
                keyColumn: "TicketTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeFrames",
                keyColumn: "TimeFrameId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeFrames",
                keyColumn: "TimeFrameId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeFrames",
                keyColumn: "TimeFrameId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TimeFrames",
                keyColumn: "TimeFrameId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TimeFrames",
                keyColumn: "TimeFrameId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "MaxOfTrainer",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "MinOfTrainee",
                table: "Classes");
        }
    }
}
