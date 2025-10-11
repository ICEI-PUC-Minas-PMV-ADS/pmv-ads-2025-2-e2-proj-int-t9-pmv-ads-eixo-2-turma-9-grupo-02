using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Doadors")]
    public class Doador 
    {
        [Key]
        public int DoadorId { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "Informe o nome do Doador")]
        [Display(Name = "Nome do Doador")]
        public string DoadorName { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 11 caracteres.")]
        [Required(ErrorMessage = "O Cpf é obrigatorio")]
        [Display(Name = "Cpf do Doador")]
        public string DoadorCpf { get; set; }

        [StringLength(100, ErrorMessage = "O tamanho não pode passar de 100 caracteres.")]
        [Required(ErrorMessage = "O Email é obrigatorio")]
        [Display(Name = "Email do Doador")]
        public string DoadorEmail { get; set; }

        [Required(ErrorMessage = "A senha é obrigatorio")]
        [Display(Name = "Senha do Doador")]
        public string DoadorSenha { get; set; }

        [Required(ErrorMessage = "Informe a data de nascimento")]
        [Display(Name = "dia/mes/ano")]
        public DateTime DoadorDataNascimento { get; set; }

        public List<Doacao> Doacaos { get; set; } 
    }
}
