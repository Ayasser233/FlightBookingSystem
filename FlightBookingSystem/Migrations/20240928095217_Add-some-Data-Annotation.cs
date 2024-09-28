using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddsomeDataAnnotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Flights",
                newName: "BookedSeats");

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Passengers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "BookedSeats",
                table: "Flights",
                newName: "AvailableSeats");
        }
    }
}
