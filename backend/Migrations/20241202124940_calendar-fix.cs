using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class calendarfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day_of_week",
                table: "timetables");

            migrationBuilder.AddColumn<long>(
                name: "calendar_id",
                table: "timetables",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "bus_number",
                table: "buses",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "wheelchair_accessible",
                table: "buses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "calendar",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    monday = table.Column<bool>(type: "boolean", nullable: false),
                    tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    thursday = table.Column<bool>(type: "boolean", nullable: false),
                    friday = table.Column<bool>(type: "boolean", nullable: false),
                    saturday = table.Column<bool>(type: "boolean", nullable: false),
                    sunday = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calendar", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_timetables_calendar_id",
                table: "timetables",
                column: "calendar_id");

            migrationBuilder.AddForeignKey(
                name: "FK_timetables_calendar_calendar_id",
                table: "timetables",
                column: "calendar_id",
                principalTable: "calendar",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timetables_calendar_calendar_id",
                table: "timetables");

            migrationBuilder.DropTable(
                name: "calendar");

            migrationBuilder.DropIndex(
                name: "IX_timetables_calendar_id",
                table: "timetables");

            migrationBuilder.DropColumn(
                name: "calendar_id",
                table: "timetables");

            migrationBuilder.DropColumn(
                name: "wheelchair_accessible",
                table: "buses");

            migrationBuilder.AddColumn<int>(
                name: "day_of_week",
                table: "timetables",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "bus_number",
                table: "buses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
