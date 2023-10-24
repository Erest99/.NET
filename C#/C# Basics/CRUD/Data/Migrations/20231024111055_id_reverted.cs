using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Data.Migrations
{
    /// <inheritdoc />
    public partial class id_reverted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "glad_id",
                table: "GladiatorModel",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "GladiatorModel",
                newName: "glad_id");
        }
    }
}
