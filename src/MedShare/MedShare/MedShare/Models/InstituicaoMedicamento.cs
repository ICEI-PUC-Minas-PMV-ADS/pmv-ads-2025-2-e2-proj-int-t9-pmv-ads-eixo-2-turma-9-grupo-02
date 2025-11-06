using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    // Relaciona uma Instituição a um Medicamento específico com sua quantidade em estoque (em caixas).
    // Criado automaticamente com quantidade = 0 para cada medicamento do catálogo quando a instituição é cadastrada.
    [Table("InstituicaoMedicamentos")]
    public class InstituicaoMedicamento
    {
        [Key]
        public int InstituicaoMedicamentoId { get; set; }

        [Required]
        public int InstituicaoId { get; set; } // FK para Instituicao
        public Instituicao Instituicao { get; set; }

        [Required]
        public int MedicamentoId { get; set; } // FK para Medicamento
        public Medicamento Medicamento { get; set; }

        [Range(0, int.MaxValue)] // Garante que não será negativa
        public int QuantidadeCaixas { get; set; }

        // Conveniência para exibir disponibilidade (true se quantidade > 0)
        public bool Disponivel => QuantidadeCaixas > 0;
    }
}
