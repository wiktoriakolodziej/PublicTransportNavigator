using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_favourite_bus_stops",
                table: "user_favourite_bus_stops");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "user_travels",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "user_favourite_bus_stops",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "user_favourite_bus_stops",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "timetables",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "ticket_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "seat_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "reserved_seats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "buses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "bus_seats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_favourite_bus_stops",
                table: "user_favourite_bus_stops",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_user_favourite_bus_stops_bus_stop_id",
                table: "user_favourite_bus_stops",
                column: "bus_stop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_favourite_bus_stops",
                table: "user_favourite_bus_stops");

            migrationBuilder.DropIndex(
                name: "IX_user_favourite_bus_stops_bus_stop_id",
                table: "user_favourite_bus_stops");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "users");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "user_travels");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_favourite_bus_stops");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "user_favourite_bus_stops");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "timetables");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "ticket_types");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "seat_types");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "reserved_seats");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "discounts");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "buses");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "bus_seats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_favourite_bus_stops",
                table: "user_favourite_bus_stops",
                columns: new[] { "bus_stop_id", "user_id" });
        }
    }
}
