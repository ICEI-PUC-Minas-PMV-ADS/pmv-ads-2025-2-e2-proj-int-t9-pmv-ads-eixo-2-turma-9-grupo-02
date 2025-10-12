using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Notificacaos")]
    public class Notificacao 
    {

        [Key]
        public int NotificacaoId { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int? DoacaoId { get; set; }
        public virtual Doacao Doacao { get; set; }
        // Adicionando relacionamento com Doador
        public int? DoadorId { get; set; }
        public virtual Doador Doador { get; set; }
        // Adicionando relacionamento com Instituicao
        public int? InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
