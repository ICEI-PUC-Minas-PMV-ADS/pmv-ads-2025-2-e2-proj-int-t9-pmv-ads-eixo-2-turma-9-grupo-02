using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AddInicialTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoadorId",
                table: "Doacoes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_DoadorId",
                table: "Doacoes",
                column: "DoadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doacoes_Doadores_DoadorId",
                table: "Doacoes",
                column: "DoadorId",
                principalTable: "Doadores",
                principalColumn: "DoadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doacoes_Doadores_DoadorId",
                table: "Doacoes");

            migrationBuilder.DropIndex(
                name: "IX_Doacoes_DoadorId",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "DoadorId",
                table: "Doacoes");
        }
    }
}
