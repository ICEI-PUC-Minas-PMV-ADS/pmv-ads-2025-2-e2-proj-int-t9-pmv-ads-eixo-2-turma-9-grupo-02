using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Instituicaos")]
    public class Instituicao 
    {
        [Key]
        public int InstituicaoId { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "Informe o nome da Instituição")]
        [Display(Name = "Nome da instituição")]
        public String InstituicaoNome { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 11 caracteres.")]
        [Required(ErrorMessage = "O Cnpj é obrigatorio")]
        [Display(Name = "Cnpj da instituição")]
        public String InstituicaoCnpj { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "O Email é obrigatorio")]
        [Display(Name = "Email da instituição")]
        public String InstituicaoEmail { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "O Endereço é obrigatorio")]
        [Display(Name = "Endereço da instituição")]
        public String InstituicaoEndereco { get; set; }

        [Required(ErrorMessage = "A senha é obrigatorio")]
        [Display(Name = "Senha do Doador")]
        public String InstituicaoSenha { get; set; }

        public List<Doacao> Doacaos { get; set; }

    }
}
