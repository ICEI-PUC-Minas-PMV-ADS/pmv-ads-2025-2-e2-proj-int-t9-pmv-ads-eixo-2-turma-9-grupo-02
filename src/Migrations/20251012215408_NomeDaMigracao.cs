using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacaos_Doacaos_DoacaoId",
                table: "Notificacaos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacaos_Instituicaos_InstituicaoId",
                table: "Notificacaos");

            migrationBuilder.RenameColumn(
                name: "NotificacaoMensagem",
                table: "Notificacaos",
                newName: "Mensagem");

            migrationBuilder.RenameColumn(
                name: "NotificacaoDataHora",
                table: "Notificacaos",
                newName: "DataCriacao");

            migrationBuilder.AlterColumn<int>(
                name: "InstituicaoId",
                table: "Notificacaos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoadorId",
                table: "Notificacaos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoacaoId",
                table: "Notificacaos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "InstituicaoPrioritaria",
                table: "Instituicaos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacaos_Doacaos_DoacaoId",
                table: "Notificacaos",
                column: "DoacaoId",
                principalTable: "Doacaos",
                principalColumn: "DoacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacaos_Instituicaos_InstituicaoId",
                table: "Notificacaos",
                column: "InstituicaoId",
                principalTable: "Instituicaos",
                principalColumn: "InstituicaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacaos_Doacaos_DoacaoId",
                table: "Notificacaos");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificacaos_Instituicaos_InstituicaoId",
                table: "Notificacaos");

            migrationBuilder.DropColumn(
                name: "InstituicaoPrioritaria",
                table: "Instituicaos");

            migrationBuilder.RenameColumn(
                name: "Mensagem",
                table: "Notificacaos",
                newName: "NotificacaoMensagem");

            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Notificacaos",
                newName: "NotificacaoDataHora");

            migrationBuilder.AlterColumn<int>(
                name: "InstituicaoId",
                table: "Notificacaos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoadorId",
                table: "Notificacaos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoacaoId",
                table: "Notificacaos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacaos_Doacaos_DoacaoId",
                table: "Notificacaos",
                column: "DoacaoId",
                principalTable: "Doacaos",
                principalColumn: "DoacaoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacaos_Instituicaos_InstituicaoId",
                table: "Notificacaos",
                column: "InstituicaoId",
                principalTable: "Instituicaos",
                principalColumn: "InstituicaoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
