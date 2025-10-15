using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class TableDoadorEInstituicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doadores",
                columns: table => new
                {
                    DoadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoadorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorCPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorSenha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.DoadorId);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstituicaoNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstituicaoEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstituicaoCNPJ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstituicaoEndereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstituicaoSenha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.InstituicaoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doadores");

            migrationBuilder.DropTable(
                name: "Instituicoes");
        }
    }
}
