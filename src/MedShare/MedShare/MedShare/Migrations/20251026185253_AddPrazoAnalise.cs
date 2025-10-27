using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AddPrazoAnalise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PrazoAnalise",
                table: "Doacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Doacoes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    NotificacaoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: true),
                    Mensagem = table.Column<string>(type: "TEXT", nullable: true),
                    Lida = table.Column<bool>(type: "INTEGER", nullable: false),
                    Horario = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.NotificacaoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "PrazoAnalise",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Doacoes");
        }
    }
}
