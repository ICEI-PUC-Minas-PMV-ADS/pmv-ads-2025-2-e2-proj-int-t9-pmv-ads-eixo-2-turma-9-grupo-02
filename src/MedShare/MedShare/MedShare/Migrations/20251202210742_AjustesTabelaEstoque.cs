using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AjustesTabelaEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Validade",
                table: "EstoqueMedicamentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "EstoqueMedicamentos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "NomeMedicamento",
                table: "EstoqueMedicamentos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "InstituicaoId",
                table: "EstoqueMedicamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueMedicamentos_InstituicaoId",
                table: "EstoqueMedicamentos",
                column: "InstituicaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EstoqueMedicamentos_Instituicoes_InstituicaoId",
                table: "EstoqueMedicamentos",
                column: "InstituicaoId",
                principalTable: "Instituicoes",
                principalColumn: "InstituicaoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstoqueMedicamentos_Instituicoes_InstituicaoId",
                table: "EstoqueMedicamentos");

            migrationBuilder.DropIndex(
                name: "IX_EstoqueMedicamentos_InstituicaoId",
                table: "EstoqueMedicamentos");

            migrationBuilder.DropColumn(
                name: "InstituicaoId",
                table: "EstoqueMedicamentos");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Validade",
                table: "EstoqueMedicamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantidade",
                table: "EstoqueMedicamentos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeMedicamento",
                table: "EstoqueMedicamentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
