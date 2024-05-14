using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addvehiclepreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "serviceTax",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "VehiclePreference",
                table: "Bookings",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VehiclePreference",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "serviceTax",
                table: "Payments",
                type: "decimal(2,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CreatedBy",
                table: "Bookings",
                column: "CreatedBy",
                unique: true,
                filter: "[CreatedBy] IS NOT NULL");
        }
    }
}
