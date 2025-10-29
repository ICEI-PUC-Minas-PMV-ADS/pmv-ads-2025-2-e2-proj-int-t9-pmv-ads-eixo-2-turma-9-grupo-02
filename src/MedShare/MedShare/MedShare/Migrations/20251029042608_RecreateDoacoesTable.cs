using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class RecreateDoacoesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doacoes_Doadores_DoadorId",
                table: "Doacoes");

            migrationBuilder.RenameColumn(
                name: "DoadorId",
                table: "Doacoes",
                newName: "DoadorID");

            migrationBuilder.RenameIndex(
                name: "IX_Doacoes_DoadorId",
                table: "Doacoes",
                newName: "IX_Doacoes_DoadorID");

            migrationBuilder.AddColumn<string>(
                name: "Doador",
                table: "Notificacoes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Notificacoes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Notificacoes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormaFarmaceutica",
                table: "Doacoes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DoadorID",
                table: "Doacoes",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoReceita",
                table: "Doacoes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doacoes_Doadores_DoadorID",
                table: "Doacoes",
                column: "DoadorID",
                principalTable: "Doadores",
                principalColumn: "DoadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doacoes_Doadores_DoadorID",
                table: "Doacoes");

            migrationBuilder.DropColumn(
                name: "Doador",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "CaminhoReceita",
                table: "Doacoes");

            migrationBuilder.RenameColumn(
                name: "DoadorID",
                table: "Doacoes",
                newName: "DoadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Doacoes_DoadorID",
                table: "Doacoes",
                newName: "IX_Doacoes_DoadorId");

            migrationBuilder.AlterColumn<string>(
                name: "FormaFarmaceutica",
                table: "Doacoes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoadorId",
                table: "Doacoes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doacoes_Doadores_DoadorId",
                table: "Doacoes",
                column: "DoadorId",
                principalTable: "Doadores",
                principalColumn: "DoadorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
