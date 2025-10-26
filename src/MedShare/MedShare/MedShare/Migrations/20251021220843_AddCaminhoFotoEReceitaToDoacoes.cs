using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AddCaminhoFotoEReceitaToDoacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoDoacao",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "ReceitaDoacao",
                table: "Doacoes");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoFoto",
                table: "Doacoes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaminhoReceita",
                table: "Doacoes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoFoto",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "CaminhoReceita",
                table: "Doacoes");

            migrationBuilder.AddColumn<string>(
                name: "FotoDoacao",
                table: "Doacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReceitaDoacao",
                table: "Doacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
