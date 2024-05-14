using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addingFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_CreatedBy",
                table: "VehicleTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_ModifiedBy",
                table: "VehicleTypes",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_CreatedBy",
                table: "VehicleDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_ModifiedBy",
                table: "VehicleDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_States_CreatedBy",
                table: "States",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_States_ModifiedBy",
                table: "States",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CreatedBy",
                table: "Ratings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ModifiedBy",
                table: "Ratings",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ModifiedBy",
                table: "Payments",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentModes_CreatedBy",
                table: "PaymentModes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentModes_ModifiedBy",
                table: "PaymentModes",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CreatedBy",
                table: "Locations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ModifiedBy",
                table: "Locations",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CreatedBy",
                table: "Drivers",
                column: "CreatedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ModifiedBy",
                table: "Drivers",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ModifiedBy",
                table: "Cities",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledBookings_CreatedBy",
                table: "CancelledBookings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledBookings_ModifiedBy",
                table: "CancelledBookings",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationReasons_CreatedBy",
                table: "CancellationReasons",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationReasons_ModifiedBy",
                table: "CancellationReasons",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStatuses_CreatedBy",
                table: "BookingStatuses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStatuses_ModifiedBy",
                table: "BookingStatuses",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings",
                column: "CreatedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ModifiedBy",
                table: "Bookings",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_CreatedBy",
                table: "Areas",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ModifiedBy",
                table: "Areas",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Users_CreatedBy",
                table: "Areas",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_Users_ModifiedBy",
                table: "Areas",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_CreatedBy",
                table: "Bookings",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_ModifiedBy",
                table: "Bookings",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStatuses_Users_CreatedBy",
                table: "BookingStatuses",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStatuses_Users_ModifiedBy",
                table: "BookingStatuses",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationReasons_Users_CreatedBy",
                table: "CancellationReasons",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CancellationReasons_Users_ModifiedBy",
                table: "CancellationReasons",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CancelledBookings_Users_CreatedBy",
                table: "CancelledBookings",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CancelledBookings_Users_ModifiedBy",
                table: "CancelledBookings",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Users_CreatedBy",
                table: "Cities",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Users_ModifiedBy",
                table: "Cities",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_CreatedBy",
                table: "Drivers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_ModifiedBy",
                table: "Drivers",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_CreatedBy",
                table: "Locations",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Users_ModifiedBy",
                table: "Locations",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentModes_Users_CreatedBy",
                table: "PaymentModes",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",  
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentModes_Users_ModifiedBy",
                table: "PaymentModes",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_CreatedBy",
                table: "Payments",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ModifiedBy",
                table: "Payments",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_CreatedBy",
                table: "Ratings",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_ModifiedBy",
                table: "Ratings",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_States_Users_CreatedBy",
                table: "States",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_States_Users_ModifiedBy",
                table: "States",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Users_CreatedBy",
                table: "VehicleDetails",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_Users_ModifiedBy",
                table: "VehicleDetails",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypes_Users_CreatedBy",
                table: "VehicleTypes",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleTypes_Users_ModifiedBy",
                table: "VehicleTypes",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Users_CreatedBy",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Areas_Users_ModifiedBy",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_ModifiedBy",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStatuses_Users_CreatedBy",
                table: "BookingStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStatuses_Users_ModifiedBy",
                table: "BookingStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationReasons_Users_CreatedBy",
                table: "CancellationReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_CancellationReasons_Users_ModifiedBy",
                table: "CancellationReasons");

            migrationBuilder.DropForeignKey(
                name: "FK_CancelledBookings_Users_CreatedBy",
                table: "CancelledBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_CancelledBookings_Users_ModifiedBy",
                table: "CancelledBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Users_CreatedBy",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Users_ModifiedBy",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_CreatedBy",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_ModifiedBy",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_CreatedBy",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Users_ModifiedBy",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentModes_Users_CreatedBy",
                table: "PaymentModes");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentModes_Users_ModifiedBy",
                table: "PaymentModes");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_CreatedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ModifiedBy",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_CreatedBy",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_ModifiedBy",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Users_CreatedBy",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Users_ModifiedBy",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Users_CreatedBy",
                table: "VehicleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_Users_ModifiedBy",
                table: "VehicleDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypes_Users_CreatedBy",
                table: "VehicleTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleTypes_Users_ModifiedBy",
                table: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_CreatedBy",
                table: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleTypes_ModifiedBy",
                table: "VehicleTypes");

            migrationBuilder.DropIndex(
                name: "IX_VehicleDetails_CreatedBy",
                table: "VehicleDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleDetails_ModifiedBy",
                table: "VehicleDetails");

            migrationBuilder.DropIndex(
                name: "IX_States_CreatedBy",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_ModifiedBy",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_CreatedBy",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_ModifiedBy",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreatedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ModifiedBy",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentModes_CreatedBy",
                table: "PaymentModes");

            migrationBuilder.DropIndex(
                name: "IX_PaymentModes_ModifiedBy",
                table: "PaymentModes");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CreatedBy",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_ModifiedBy",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_CreatedBy",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ModifiedBy",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CreatedBy",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_ModifiedBy",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_CancelledBookings_CreatedBy",
                table: "CancelledBookings");

            migrationBuilder.DropIndex(
                name: "IX_CancelledBookings_ModifiedBy",
                table: "CancelledBookings");

            migrationBuilder.DropIndex(
                name: "IX_CancellationReasons_CreatedBy",
                table: "CancellationReasons");

            migrationBuilder.DropIndex(
                name: "IX_CancellationReasons_ModifiedBy",
                table: "CancellationReasons");

            migrationBuilder.DropIndex(
                name: "IX_BookingStatuses_CreatedBy",
                table: "BookingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_BookingStatuses_ModifiedBy",
                table: "BookingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ModifiedBy",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Areas_CreatedBy",
                table: "Areas");

            migrationBuilder.DropIndex(
                name: "IX_Areas_ModifiedBy",
                table: "Areas");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedBy",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
