using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class quicfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buses_bus_stops_first_bus_stop_id",
                table: "buses");

            migrationBuilder.DropForeignKey(
                name: "FK_buses_bus_stops_last_bus_stop_id",
                table: "buses");

            migrationBuilder.AddForeignKey(
                name: "FK_buses_bus_stops_first_bus_stop_id",
                table: "buses",
                column: "first_bus_stop_id",
                principalTable: "bus_stops",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_buses_bus_stops_last_bus_stop_id",
                table: "buses",
                column: "last_bus_stop_id",
                principalTable: "bus_stops",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buses_bus_stops_first_bus_stop_id",
                table: "buses");

            migrationBuilder.DropForeignKey(
                name: "FK_buses_bus_stops_last_bus_stop_id",
                table: "buses");

            migrationBuilder.AddForeignKey(
                name: "FK_buses_bus_stops_first_bus_stop_id",
                table: "buses",
                column: "first_bus_stop_id",
                principalTable: "bus_stops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_buses_bus_stops_last_bus_stop_id",
                table: "buses",
                column: "last_bus_stop_id",
                principalTable: "bus_stops",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
