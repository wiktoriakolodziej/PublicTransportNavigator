using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class TicketTypeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reserved_seat_timetables_timetable_in_id",
                table: "reserved_seat");

            migrationBuilder.DropForeignKey(
                name: "FK_reserved_seat_timetables_timetable_off_id",
                table: "reserved_seat");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "time_range",
                table: "ticket_types",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "ticket_types",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reserved_seat_timetables_timetable_in_id",
                table: "reserved_seat",
                column: "timetable_in_id",
                principalTable: "timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_reserved_seat_timetables_timetable_off_id",
                table: "reserved_seat",
                column: "timetable_off_id",
                principalTable: "timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reserved_seat_timetables_timetable_in_id",
                table: "reserved_seat");

            migrationBuilder.DropForeignKey(
                name: "FK_reserved_seat_timetables_timetable_off_id",
                table: "reserved_seat");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "time_range",
                table: "ticket_types",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<float>(
                name: "price",
                table: "ticket_types",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddForeignKey(
                name: "FK_reserved_seat_timetables_timetable_in_id",
                table: "reserved_seat",
                column: "timetable_in_id",
                principalTable: "timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_reserved_seat_timetables_timetable_off_id",
                table: "reserved_seat",
                column: "timetable_off_id",
                principalTable: "timetables",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
