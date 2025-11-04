using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedShare.Models; // Para usar o enum StatusDoacao

namespace MedShare.Models
{
    [Table("Doacoes")]
    public class Doacao
    {
        [Key]
        public  int Id { get; set; }

        [Required(ErrorMessage ="Obrigatório informar o nome do medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        public string NomeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a validade do medicamento!")]
        [Display(Name = "Validade")]
        public DateOnly? ValidadeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a quantidade do medicamento!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        [Display(Name = "Quantidade (caixa)")]
        public int? QuantidadeDoacao { get; set; }

        // Guarda o caminho do arquivo no banco de dados
        [Display(Name = "Foto do Medicamento")]
        public string? CaminhoFoto { get; set; }

        [Display(Name = "Receita")]
        public string? CaminhoReceita { get; set; }

        // Para upload do arquivo (não salva no banco, e sim no wwwroot/images)
        [NotMapped]
        [Display(Name = "Foto do Medicamento")]
        public IFormFile? FotoDoacao { get; set; }

        [NotMapped]
        [Display(Name = "Receita")]
        public IFormFile? ReceitaDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório selecionar a instituição!")]
        [Display(Name = "Instituição")]
        public int InstituicaoId { get; set; }

        [ForeignKey("InstituicaoId")]
        public Instituicao Instituicao { get; set; }

        [Display(Name = "Status")]
        public StatusDoacao Status { get; set; } = StatusDoacao.Pendente;

        // Relacionamento da doação com o doador

        [Display(Name = "Doador")]
        public int? DoadorId { get; set; }

        [ForeignKey("DoadorId")]
        public Doador? Doador { get; set; }
    }
}