using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    // Modelo que representa um usuário e seus perfis de acesso.

    [Table("Usuarios")]
    public class Usuario 
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage ="Obrigatorio email!")]
        [Display(Name ="Email")]
        public string UsuarioEmail { get; set; }

        [Required(ErrorMessage = "Obrigatorio Senha!")]
        [DataType(DataType.Password)] /*Essa dataAnotation mascara essa senha*/
        [Display(Name = "Senha")]
        public string UsuarioSenha { get; set; }

        [Required(ErrorMessage = "Obrigatorio selecionar perfil Antes")]
        public Perfil Perfil { get; set; }

        public static Usuario GetAdminUsuario()
        {
            return new Usuario
            {
                UsuarioEmail = "admin@medshare.com",
                UsuarioSenha = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Perfil = Perfil.Admin
            };
        }
    }

    /*Vamos criar os perfis de acesso e autenticação*/
    public enum Perfil 
    {   /*Filtro de acesso ao sistema*/
        Admin, /*Perfil de acesso para gerencia*/
        Doador, /*Perfil de acesso que pode não pode deletar nem o seu e nenum outro user */
        Instituicao /*Perfil de acesso que pode não pode deletar nem o seu e nenum outro user */
    }
}
