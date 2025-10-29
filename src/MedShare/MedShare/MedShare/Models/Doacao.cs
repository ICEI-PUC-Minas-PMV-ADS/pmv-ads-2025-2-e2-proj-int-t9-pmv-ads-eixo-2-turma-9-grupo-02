using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("Doacoes")]
    public class Doacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o nome do medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        public string NomeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a validade do medicamento!")]
        [Display(Name = "ValidadeDoacao")]
        [DataType(DataType.Date)] 
        public DateOnly ValidadeDoacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a quantidade do medicamento!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero!")]
        [Display(Name = "QuantidadeDoacao")]
        public int QuantidadeDoacao { get; set; }

        [NotMapped]
        [Display(Name = "Foto da Doação")]
        public IFormFile? FotoDoacao { get; set; } 

        [NotMapped]
        [Display(Name = "Receita Médica")]
        public IFormFile? ReceitaDoacao { get; set; } 

        [Display(Name = "Caminho da Foto")]
        public string? CaminhoFoto { get; set; }

        [Display(Name = "Caminho da Receita")]
        public string? CaminhoReceita { get; set; }

        [Display(Name = "Status da Doação")]
        public string Status { get; set; } = "Disponível";

        [Display(Name = "Prazo para Análise")]
        public DateTime PrazoAnalise { get; set; } = DateTime.Now.AddHours(48);

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Forma Farmaceutica")]
        public string? FormaFarmaceutica { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        public int? DoadorID { get; set; }

        [ForeignKey("DoadorID")]
        public Doador? Doador { get; set; }
    }
}

