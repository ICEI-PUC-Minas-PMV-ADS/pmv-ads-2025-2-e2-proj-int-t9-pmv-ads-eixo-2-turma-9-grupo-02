using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Doadors")]
    public class Doador 
    {
        [Key]
        public int DoadorId { get; set; }

        [Required(ErrorMessage ="Obrigatorio informar nome!")]
        public string DoadorName { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar cpf!")]
        public string DoadorCpf { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar email!")]
        public string DoadorEmail{ get; set; }

        [Required(ErrorMessage = "Obrigatorio informar data de nascimento!")]
        public DateOnly DoadorDataNascimento { get; set; }

        [Required(ErrorMessage = "Obrigatorio informar senha!")]
        public string DoadorSenha { get; set; }
    }
}
