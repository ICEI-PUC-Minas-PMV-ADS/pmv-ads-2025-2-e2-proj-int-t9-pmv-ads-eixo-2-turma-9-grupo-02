using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("MedicamentoEstoques")]
    public class MedicamentoEstoque 
    {
        [Key]
        public int MedicamentoEstoqueId { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "Informe o nome do medicamento")]
        [Display(Name = "Nome do medicamento")]
        public String MedicamentoEstoqueNome { get; set; }
        public int MedicamentoEstoqueQuantidade { get; set; }
        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
