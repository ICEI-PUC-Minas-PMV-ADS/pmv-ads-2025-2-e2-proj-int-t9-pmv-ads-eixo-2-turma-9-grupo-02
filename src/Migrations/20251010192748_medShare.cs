using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class medShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doadors",
                columns: table => new
                {
                    DoadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoadorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DoadorCpf = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DoadorEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DoadorSenha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorDataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadors", x => x.DoadorId);
                });

            migrationBuilder.CreateTable(
                name: "Instituicaos",
                columns: table => new
                {
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstituicaoNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituicaoCnpj = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituicaoEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituicaoEndereco = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InstituicaoSenha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicaos", x => x.InstituicaoId);
                });

            migrationBuilder.CreateTable(
                name: "Doacaos",
                columns: table => new
                {
                    DoacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoacaoNomeMedicamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DoacaoDataValidade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoacaoQuantidadeMedicamento = table.Column<int>(type: "int", nullable: false),
                    DoacaoUrlReceita = table.Column<int>(type: "int", nullable: false),
                    DoacaoUrlFoto = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DoadorId = table.Column<int>(type: "int", nullable: false),
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacaos", x => x.DoacaoId);
                    table.ForeignKey(
                        name: "FK_Doacaos_Doadors_DoadorId",
                        column: x => x.DoadorId,
                        principalTable: "Doadors",
                        principalColumn: "DoadorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doacaos_Instituicaos_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicaos",
                        principalColumn: "InstituicaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentoEstoques",
                columns: table => new
                {
                    MedicamentoEstoqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicamentoEstoqueNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MedicamentoEstoqueQuantidade = table.Column<int>(type: "int", nullable: false),
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentoEstoques", x => x.MedicamentoEstoqueId);
                    table.ForeignKey(
                        name: "FK_MedicamentoEstoques_Instituicaos_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicaos",
                        principalColumn: "InstituicaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoDoacaos",
                columns: table => new
                {
                    HistoricoDoacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoricoDoacaoDataFinalizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoricoDoacaoResultado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoDoacaos", x => x.HistoricoDoacaoId);
                    table.ForeignKey(
                        name: "FK_HistoricoDoacaos_Doacaos_DoacaoId",
                        column: x => x.DoacaoId,
                        principalTable: "Doacaos",
                        principalColumn: "DoacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacaos",
                columns: table => new
                {
                    NotificacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificacaoMensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificacaoDataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoacaoId = table.Column<int>(type: "int", nullable: false),
                    DoadorId = table.Column<int>(type: "int", nullable: false),
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacaos", x => x.NotificacaoId);
                    table.ForeignKey(
                        name: "FK_Notificacaos_Doacaos_DoacaoId",
                        column: x => x.DoacaoId,
                        principalTable: "Doacaos",
                        principalColumn: "DoacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacaos_Doadors_DoadorId",
                        column: x => x.DoadorId,
                        principalTable: "Doadors",
                        principalColumn: "DoadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notificacaos_Instituicaos_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicaos",
                        principalColumn: "InstituicaoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doacaos_DoadorId",
                table: "Doacaos",
                column: "DoadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Doacaos_InstituicaoId",
                table: "Doacaos",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoDoacaos_DoacaoId",
                table: "HistoricoDoacaos",
                column: "DoacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentoEstoques_InstituicaoId",
                table: "MedicamentoEstoques",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacaos_DoacaoId",
                table: "Notificacaos",
                column: "DoacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacaos_DoadorId",
                table: "Notificacaos",
                column: "DoadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacaos_InstituicaoId",
                table: "Notificacaos",
                column: "InstituicaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoDoacaos");

            migrationBuilder.DropTable(
                name: "MedicamentoEstoques");

            migrationBuilder.DropTable(
                name: "Notificacaos");

            migrationBuilder.DropTable(
                name: "Doacaos");

            migrationBuilder.DropTable(
                name: "Doadors");

            migrationBuilder.DropTable(
                name: "Instituicaos");
        }
    }
}
