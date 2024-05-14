using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class upgrading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentModes_PaymentModeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentModeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentModeId",
                table: "Payments");

            migrationBuilder.AddColumn<decimal>(
                name: "serviceTax",
                table: "Payments",
                type: "decimal(2,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Locations",
                type: "varchar(11)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "Lattitude",
                table: "Locations",
                type: "varchar(11)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Bookings",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ModeId",
                table: "Payments",
                column: "ModeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentModes_ModeId",
                table: "Payments",
                column: "ModeId",
                principalTable: "PaymentModes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentModes_ModeId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ModeId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "serviceTax",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "PaymentModeId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Lattitude",
                table: "Locations",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentModeId",
                table: "Payments",
                column: "PaymentModeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentModes_PaymentModeId",
                table: "Payments",
                column: "PaymentModeId",
                principalTable: "PaymentModes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
