using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("Doacoes")]
    public class Doacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do medicamento é obrigatório")]
        [Display(Name = "Nome do Medicamento")]
        public string NomeMedicamento { get; set; }

        [Required(ErrorMessage = "A validade é obrigatória")]
        [Display(Name = "Validade")]
        [DataType(DataType.Date)]
        public DateTime Validade { get; set; }

        [Required(ErrorMessage = "A forma farmacêutica é obrigatória")]
        [Display(Name = "Forma Farmacêutica")]
        public string FormaFarmaceutica { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Display(Name = "Quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Display(Name = "Descrição/Comentário")]
        public string Descricao { get; set; }

        [Display(Name = "Imagem")]
        public string ImagemPath { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Display(Name = "Status da Doação")]
        public string Status { get; set; } = "Disponível";

        [Display(Name = "Prazo para Análise")]
        public DateTime PrazoAnalise { get; set; } = DateTime.Now.AddHours(48);

        // Relacionamento com doador
        public int DoadorId { get; set; }
        public Doador Doador { get; set; }
    }
}