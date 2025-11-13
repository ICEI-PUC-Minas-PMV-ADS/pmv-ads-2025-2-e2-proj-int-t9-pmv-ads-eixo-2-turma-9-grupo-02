using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    // Medicamento vinculado a uma Instituição
    [Table("MedicamentosInstituicao")]
    public class MedicamentoInstituicao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InstituicaoId { get; set; }

        [ForeignKey(nameof(InstituicaoId))]
        public Instituicao Instituicao { get; set; }

        [Required(ErrorMessage = "Obrigatório Nome do Medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Obrigatório informar quantidade de caixas!")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantidade deve ser >= 0")]
        [Display(Name = "Quantidade de Caixas")]
        public int QuantidadeCaixas { get; set; }

        [Display(Name = "Observação")]
        public string? Observacao { get; set; }

        // Indica se está em falta (quantidade = 0)
        [NotMapped]
        public bool EmFalta => QuantidadeCaixas == 0;

        // Indica escassez crítica (menos de 11 caixas)
        [NotMapped]
        public bool EscassezCritica => QuantidadeCaixas < 11;
    }
}
