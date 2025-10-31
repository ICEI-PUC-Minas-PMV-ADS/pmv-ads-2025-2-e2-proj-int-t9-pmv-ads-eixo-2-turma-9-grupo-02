using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    // Modelo que representa um doador no sistema.

    [Table("Doadores")]
    public class Doador {

        [Key]
        public int DoadorId { get; set; }

        [Required(ErrorMessage = "Obrigatorio Nome!")]
        [Display(Name = "Nome")]
        public string DoadorNome { get; set; }

        [Required(ErrorMessage = "Obrigatorio Email!")]
        [Display(Name = "Email")]
        public string DoadorEmail { get; set; }

        [Required(ErrorMessage = "Obrigatorio CPF!")]
        [Display(Name = "CPF")]
        public string DoadorCPF { get; set; }

        [Required(ErrorMessage = "Obrigatorio Senha!")]
        [Display(Name = "Senha")]
        public string DoadorSenha { get; set; }
    }

}
