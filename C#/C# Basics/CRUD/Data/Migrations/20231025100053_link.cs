using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD.Data.Migrations
{
    /// <inheritdoc />
    public partial class link : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "GladiatorModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.user_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GladiatorModel_user_id",
                table: "GladiatorModel",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GladiatorModel_UserModel_user_id",
                table: "GladiatorModel",
                column: "user_id",
                principalTable: "UserModel",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GladiatorModel_UserModel_user_id",
                table: "GladiatorModel");

            migrationBuilder.DropTable(
                name: "UserModel");

            migrationBuilder.DropIndex(
                name: "IX_GladiatorModel_user_id",
                table: "GladiatorModel");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "GladiatorModel");
        }
    }
}
