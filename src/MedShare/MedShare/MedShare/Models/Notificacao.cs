using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models
{
    [Table("Notificacoes")]
    public class Notificacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DoadorId { get; set; }

        [ForeignKey("DoadorId")]
        public Doador Doador { get; set; }

        [Required]
        [MaxLength(200)]
        public string Mensagem { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Lida { get; set; } = false;
    }
}
