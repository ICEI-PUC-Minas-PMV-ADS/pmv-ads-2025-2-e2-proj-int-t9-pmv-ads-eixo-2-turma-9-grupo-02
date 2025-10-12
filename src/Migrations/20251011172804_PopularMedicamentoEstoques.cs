using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedShare.Migrations
{
    /// <inheritdoc />
    public partial class PopularMedicamentoEstoques : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            // Insere as instituições (o Id será gerado automaticamente)
            migrationBuilder.Sql("INSERT INTO Instituicaos(InstituicaoNome, InstituicaoCnpj, InstituicaoEmail, InstituicaoEndereco, InstituicaoSenha) VALUES " +
                "('Instituição Y', '00000000005', 'Y@email.com', 'Endereço Z', 'senhaY')");
            migrationBuilder.Sql("INSERT INTO Instituicaos(InstituicaoNome, InstituicaoCnpj, InstituicaoEmail, InstituicaoEndereco, InstituicaoSenha) VALUES " +
                "('Instituição Z', '00000000006', 'Z@email.com', 'Endereço Y', 'senhaZ')");

            // Insere os medicamentos, referenciando o Id gerado das instituições
            migrationBuilder.Sql("INSERT INTO MedicamentoEstoques(MedicamentoEstoqueNome,MedicamentoEstoqueQuantidade,InstituicaoId) SELECT " +
                "'Paracetamol', 10, InstituicaoId FROM Instituicaos WHERE InstituicaoNome = 'Instituição Y'");
            migrationBuilder.Sql("INSERT INTO MedicamentoEstoques(MedicamentoEstoqueNome,MedicamentoEstoqueQuantidade,InstituicaoId) SELECT " +
                "'Ibuprofeno', 20, InstituicaoId FROM Instituicaos WHERE InstituicaoNome = 'Instituição Z'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DELETE FROM MedicamentoEstoques");
        }
    }
}
