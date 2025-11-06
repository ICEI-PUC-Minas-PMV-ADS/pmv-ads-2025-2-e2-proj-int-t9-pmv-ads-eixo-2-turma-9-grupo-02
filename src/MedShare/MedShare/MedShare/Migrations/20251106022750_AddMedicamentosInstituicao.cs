using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicamentosInstituicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    MedicamentoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.MedicamentoId);
                });

            migrationBuilder.CreateTable(
                name: "InstituicaoMedicamentos",
                columns: table => new
                {
                    InstituicaoMedicamentoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InstituicaoId = table.Column<int>(type: "INTEGER", nullable: false),
                    MedicamentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantidadeCaixas = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituicaoMedicamentos", x => x.InstituicaoMedicamentoId);
                    table.ForeignKey(
                        name: "FK_InstituicaoMedicamentos_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "InstituicaoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstituicaoMedicamentos_Medicamentos_MedicamentoId",
                        column: x => x.MedicamentoId,
                        principalTable: "Medicamentos",
                        principalColumn: "MedicamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Medicamentos",
                columns: new[] { "MedicamentoId", "Nome" },
                values: new object[,]
                {
                    { 1, "Dipirona" },
                    { 2, "Paracetamol" },
                    { 3, "Amoxicilina" },
                    { 4, "Omeprazol" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstituicaoMedicamentos_InstituicaoId_MedicamentoId",
                table: "InstituicaoMedicamentos",
                columns: new[] { "InstituicaoId", "MedicamentoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstituicaoMedicamentos_MedicamentoId",
                table: "InstituicaoMedicamentos",
                column: "MedicamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstituicaoMedicamentos");

            migrationBuilder.DropTable(
                name: "Medicamentos");
        }
    }
}
