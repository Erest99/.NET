using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedxpandrest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastrested",
                table: "GladiatorModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "level",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xp",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "xptolevel",
                table: "GladiatorModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastrested",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "level",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "xp",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "xptolevel",
                table: "GladiatorModel");
        }
    }
}
