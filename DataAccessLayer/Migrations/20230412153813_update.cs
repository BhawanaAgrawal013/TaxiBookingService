using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CancelledBookings_CancellationReasons_CancellationReasonid",
                table: "CancelledBookings");

            migrationBuilder.DropIndex(
                name: "IX_CancelledBookings_CancellationReasonid",
                table: "CancelledBookings");

            migrationBuilder.DropColumn(
                name: "CancellationReasonid",
                table: "CancelledBookings");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "CancelledBookings",
                newName: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledBookings_ReasonId",
                table: "CancelledBookings",
                column: "ReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CancelledBookings_CancellationReasons_ReasonId",
                table: "CancelledBookings",
                column: "ReasonId",
                principalTable: "CancellationReasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CancelledBookings_CancellationReasons_ReasonId",
                table: "CancelledBookings");

            migrationBuilder.DropIndex(
                name: "IX_CancelledBookings_ReasonId",
                table: "CancelledBookings");

            migrationBuilder.RenameColumn(
                name: "ReasonId",
                table: "CancelledBookings",
                newName: "Reason");

            migrationBuilder.AddColumn<int>(
                name: "CancellationReasonid",
                table: "CancelledBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CancelledBookings_CancellationReasonid",
                table: "CancelledBookings",
                column: "CancellationReasonid");

            migrationBuilder.AddForeignKey(
                name: "FK_CancelledBookings_CancellationReasons_CancellationReasonid",
                table: "CancelledBookings",
                column: "CancellationReasonid",
                principalTable: "CancellationReasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
