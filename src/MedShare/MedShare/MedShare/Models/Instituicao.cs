using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    // Modelo que representa uma instituição no sistema.

    [Table("Instituicoes")]
    public class Instituicao {

        [Key]
        public int InstituicaoId { get; set; }

        [Required(ErrorMessage = "Obrigatorio Nome!")]
        [Display(Name = "Nome")]
        public string InstituicaoNome { get; set; }

        [Required(ErrorMessage = "Obrigatorio Email!")]
        [Display(Name = "Email")]
        public string InstituicaoEmail { get; set; }

        [Required(ErrorMessage = "Obrigatorio CNPJ!")]
        [Display(Name = "CNPJ")]
        public string InstituicaoCNPJ { get; set; }

        [Required(ErrorMessage = "Obrigatorio Endereço!")]
        [Display(Name = "Endereço")]
        public string InstituicaoEndereco { get; set; }

        [Required(ErrorMessage = "Obrigatorio Senha!")]
        [Display(Name = "Senha")]
        public string InstituicaoSenha { get; set; }
    }
}    
