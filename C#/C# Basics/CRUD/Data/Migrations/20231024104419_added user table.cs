using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "GladiatorModel",
                newName: "glad_name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GladiatorModel",
                newName: "glad_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "glad_name",
                table: "GladiatorModel",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "glad_id",
                table: "GladiatorModel",
                newName: "id");
        }
    }
}
