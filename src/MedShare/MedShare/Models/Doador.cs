using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Doadors")]
    public class Doador 
    {
        [Key]
        public int DoadorId { get; set; }

        [Required(ErrorMessage ="Obrigatorio informar nome!")]
        [Display(Name = "Nome Doador")]
        public string DoadorName { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar cpf!")]
        [Display(Name = "CPF")]
        public string DoadorCpf { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar email!")]
        [Display(Name = "Email")]
        public string DoadorEmail{ get; set; }

        [Required(ErrorMessage = "Obrigatorio informar data de nascimento!")]
        [Display(Name = "Data Nascimento")]
        public DateOnly DoadorDataNascimento { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar senha!")]
        [Display(Name = "Senha do Doador")]
        public string DoadorSenha { get; set; }
    }
}
