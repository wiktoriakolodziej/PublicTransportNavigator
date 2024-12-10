using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublicTransportNavigator.Migrations
{
    /// <inheritdoc />
    public partial class fix_seats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "seat_position",
                table: "bus_seats");

            migrationBuilder.RenameColumn(
                name: "icon",
                table: "seat_types",
                newName: "icon_path");

            migrationBuilder.AlterColumn<string>(
                name: "icon_path",
                table: "seat_types",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "x_offset",
                table: "bus_seats",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "y_offset",
                table: "bus_seats",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "x_offset",
                table: "bus_seats");

            migrationBuilder.DropColumn(
                name: "y_offset",
                table: "bus_seats");

            migrationBuilder.RenameColumn(
                name: "icon_path",
                table: "seat_types",
                newName: "icon");

            migrationBuilder.AlterColumn<byte[]>(
                name: "icon",
                table: "seat_types",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "seat_position",
                table: "bus_seats",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
