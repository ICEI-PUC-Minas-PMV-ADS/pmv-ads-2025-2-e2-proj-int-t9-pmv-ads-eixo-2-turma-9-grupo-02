using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("EstoqueMedicamentos")]
    public class EstoqueMedicamento
    {
        public EstoqueMedicamento()
        {
            QuantidadeMinima = 15;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o nome do medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        public string NomeMedicamento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a validade do medicamento!")]
        [Display(Name = "Data de Validade")]
        public DateOnly? Validade { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a quantidade!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        [Display(Name = "Quantidade (caixa)")]
        public int? Quantidade { get; set; }

        [NotMapped]
        public int QuantidadeMinima { get; private set; }

        [Required]
        public int InstituicaoId { get; set; }

        [ForeignKey("InstituicaoId")]
        public Instituicao Instituicao { get; set; }
    }
}