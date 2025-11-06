using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
<<<<<<< Updated upstream
    // Representa um item do cat�logo global de medicamentos.
    // Cada institui��o ganha um registro de estoque (InstituicaoMedicamento) para cada Medicamento deste cat�logo.
=======
>>>>>>> Stashed changes
    [Table("Medicamentos")]
    public class Medicamento
    {
        [Key]
<<<<<<< Updated upstream
        public int MedicamentoId { get; set; }

        [Required]
        [StringLength(100)] // Limita tamanho do nome para consist�ncia e performance de �ndices.
        public string Nome { get; set; }
    }
}
=======
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o nome do medicamento!")]
        [Display(Name = "Nome do Medicamento")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a categoria!")]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a dosagem!")]
        [Display(Name = "Dosagem")]
        public string Dosagem { get; set; }

        [Display(Name = "Forma Farmacêutica")]
        public string FormaFarmaceutica { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o nível de prioridade!")]
        [Display(Name = "Nível de Prioridade")]
        [Range(1, 5, ErrorMessage = "A prioridade deve ser entre 1 (baixa) e 5 (alta).")]
        public int NivelPrioridade { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a quantidade necessária!")]
        [Display(Name = "Quantidade Necessária")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int QuantidadeNecessaria { get; set; }

        [Display(Name = "Quantidade Atual em Estoque")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser zero ou maior.")]
        public int QuantidadeAtual { get; set; } = 0;

        [Display(Name = "Status de Escassez")]
        public bool EmEscassez => QuantidadeAtual < (QuantidadeNecessaria * 0.3);

        [Required(ErrorMessage = "Obrigatório selecionar a instituição!")]
        [Display(Name = "Instituição")]
        public int InstituicaoId { get; set; }

        [ForeignKey("InstituicaoId")]
        public Instituicao Instituicao { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Display(Name = "Última Atualização")]
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Observações")]
        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo 1000 caracteres.")]
        public string Observacoes { get; set; }
    }
}
>>>>>>> Stashed changes
