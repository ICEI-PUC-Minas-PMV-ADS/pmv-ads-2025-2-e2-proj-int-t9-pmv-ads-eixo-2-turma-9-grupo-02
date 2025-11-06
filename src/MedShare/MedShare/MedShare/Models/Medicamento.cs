using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    // Representa um item do catálogo global de medicamentos.
    // Cada instituição ganha um registro de estoque (InstituicaoMedicamento) para cada Medicamento deste catálogo.
    [Table("Medicamentos")]
    public class Medicamento
    {
        [Key]
        public int MedicamentoId { get; set; }

        [Required]
        [StringLength(100)] // Limita tamanho do nome para consistência e performance de índices.
        public string Nome { get; set; }
    }
}
