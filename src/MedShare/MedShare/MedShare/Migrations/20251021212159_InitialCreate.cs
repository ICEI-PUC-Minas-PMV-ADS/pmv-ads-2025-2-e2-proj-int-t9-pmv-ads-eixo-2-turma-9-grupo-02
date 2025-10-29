using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doadores",
                columns: table => new
                {
                    DoadorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DoadorNome = table.Column<string>(type: "TEXT", nullable: false),
                    DoadorEmail = table.Column<string>(type: "TEXT", nullable: false),
                    DoadorCPF = table.Column<string>(type: "TEXT", nullable: false),
                    DoadorSenha = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.DoadorId);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    InstituicaoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstituicaoNome = table.Column<string>(type: "TEXT", nullable: false),
                    InstituicaoEmail = table.Column<string>(type: "TEXT", nullable: false),
                    InstituicaoCNPJ = table.Column<string>(type: "TEXT", nullable: false),
                    InstituicaoEndereco = table.Column<string>(type: "TEXT", nullable: false),
                    InstituicaoSenha = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.InstituicaoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Doador = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioEmail = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioSenha = table.Column<string>(type: "TEXT", nullable: false),
                    Perfil = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Doador);
                });

            migrationBuilder.CreateTable(
                name: "Doacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomeDoacao = table.Column<string>(type: "TEXT", nullable: false),
                    ValidadeDoacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FormaFarmaceutica = table.Column<string>(type: "TEXT", nullable: false),
                    QuantidadeDoacao = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    CaminhoFoto = table.Column<string>(type: "TEXT", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DoadorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doacoes_Doadores_DoadorId",
                        column: x => x.DoadorId,
                        principalTable: "Doadores",
                        principalColumn: "DoadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_DoadorId",
                table: "Doacoes",
                column: "DoadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doacoes");

            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Doadores");
        }
    }
}
