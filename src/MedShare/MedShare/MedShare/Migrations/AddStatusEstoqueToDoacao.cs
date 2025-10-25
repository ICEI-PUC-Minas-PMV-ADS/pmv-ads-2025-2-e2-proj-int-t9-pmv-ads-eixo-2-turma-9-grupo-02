using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusEstoqueToDoacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstituicaoId",
                table: "Doacoes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusEstoque",
                table: "Doacoes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_InstituicaoId",
                table: "Doacoes",
                column: "InstituicaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doacoes_Instituicoes_InstituicaoId",
                table: "Doacoes",
                column: "InstituicaoId",
                principalTable: "Instituicoes",
                principalColumn: "InstituicaoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doacoes_Instituicoes_InstituicaoId",
                table: "Doacoes");

            migrationBuilder.DropIndex(
                name: "IX_Doacoes_InstituicaoId",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "InstituicaoId",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "StatusEstoque",
                table: "Doacoes");
        }
    }
}
