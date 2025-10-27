using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("Notificacoes")]
    public class Notificacao
    {
        [Key]
        public int NotificacaoId { get; set; }
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public bool Lida { get; set; }
        public DateTime Horario { get; set; }
        public string Tipo { get; set; } // "sucesso", "erro", "aviso", "informação"
    }
}
