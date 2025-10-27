using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("Doacoes")]
    public class Doacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Obrigatório informar o nome do medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        public string NomeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a validade do medicamento!")]
        [Display(Name = "Validade")]
        public DateOnly ValidadeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a quantidade do medicamento!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero!")]
        [Display(Name = "Quantidade")]
        public int QuantidadeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório enviar foto da doação!")]
        [Display(Name = "Foto")]
        [NotMapped] 
        public IFormFile FotoDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório enviar a receita do medicamento!")]
        [Display(Name = "Receita")]
        [NotMapped]
        public IFormFile ReceitaDoacao { get; set; }
        
        [Display(Name = "Status da Doação")]
        public string Status { get; set; } = "Disponível";

        [Display(Name = "Prazo para Análise")]
        public DateTime PrazoAnalise { get; set; } = DateTime.Now.AddHours(48);

        public string CaminhoFoto { get; set; }
        public string CaminhoReceita { get; set; }
    }
}
