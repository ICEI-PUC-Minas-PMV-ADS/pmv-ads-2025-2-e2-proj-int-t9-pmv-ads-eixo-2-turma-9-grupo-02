using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class TableDoador : Migration
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
                    DoadorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorCpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoadorDataNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    DoadorSenha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadors", x => x.DoadorId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doadors");
        }
    }
}
