using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class M01_addTableDoacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeDoacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidadeDoacao = table.Column<DateOnly>(type: "date", nullable: false),
                    QuantidadeDoacao = table.Column<int>(type: "int", nullable: false),
                    FotoDoacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceitaDoacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacoes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doacoes");
        }
    }
}
