using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Doacaos")]
    public class Doacao 
    {
        [Key]
        public int DoacaoId { get; set; }

        [StringLength(100, ErrorMessage ="O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage ="Informe o nome do medicamento")]
        [Display(Name ="Nome do Medicamento")]
        public string DoacaoNomeMedicamento { get; set; }

        [Required(ErrorMessage = "Informe a data de validade do medicamento")]
        [Display(Name = "dia/mes/ano")]
        public DateTime DoacaoDataValidade { get; set; }

        [Required(ErrorMessage = "Informe a quantidade")]
        [Display(Name = "quantidade")]
        public int DoacaoQuantidadeMedicamento { get; set; }

        [Required(ErrorMessage = "Anexe a receita do medicamento")]
        public int DoacaoUrlReceita { get; set; }

        [Required(ErrorMessage = "Anexe a receita do medicamento")]
        public int DoacaoUrlFoto { get; set; }
        public DoacaoStatus Status { get; set; }
        /* O certo para manter o padrão de nomes teria que ser public DoacaoStatus DoacaoStatus { get; set; } */

        public int DoadorId { get; set; }
        public virtual Doador Doador { get; set; }
        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
