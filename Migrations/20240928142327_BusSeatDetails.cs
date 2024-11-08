using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class BusSeatDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "seat_position",
                table: "bus_seats",
                nullable: false,
                defaultValue: "standing", // Setting default value
                oldClrType: typeof(string), // Assuming it was previously set to interval
                oldNullable: false); // Specify if it was nullable or not
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
