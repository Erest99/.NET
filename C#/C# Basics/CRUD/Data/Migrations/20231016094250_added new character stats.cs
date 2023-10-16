using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Data.Migrations
{
    /// <inheritdoc />
    public partial class addednewcharacterstats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "defence",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "maxhealth",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "speed",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "defence",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "maxhealth",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "speed",
                table: "GladiatorModel");
        }
    }
}
