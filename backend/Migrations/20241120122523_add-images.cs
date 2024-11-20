using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class addimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "buses");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "bus_arrival_time",
                table: "timetables",
                type: "interval",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<long>(
                name: "bus_type_id",
                table: "buses",
                type: "bigint",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "bus_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bus_types", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_buses_bus_type_id",
                table: "buses",
                column: "bus_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_buses_bus_types_bus_type_id",
                table: "buses",
                column: "bus_type_id",
                principalTable: "bus_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_buses_bus_types_bus_type_id",
                table: "buses");

            migrationBuilder.DropTable(
                name: "bus_types");

            migrationBuilder.DropIndex(
                name: "IX_buses_bus_type_id",
                table: "buses");

            migrationBuilder.DropColumn(
                name: "bus_type_id",
                table: "buses");

            migrationBuilder.AlterColumn<DateTime>(
                name: "bus_arrival_time",
                table: "timetables",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "buses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
