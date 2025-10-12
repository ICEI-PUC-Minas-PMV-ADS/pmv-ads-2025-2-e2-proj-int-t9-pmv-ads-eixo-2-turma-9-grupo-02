using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("Notificacaos")]
    public class Notificacao 
    {

        [Key]
        public int NotificacaoId { get; set; }
        public string NotificacaoMensagem { get; set; }
        public DateTime NotificacaoDataHora { get; set; }
        public int DoacaoId { get; set; }
        public virtual Doacao Doacao { get; set; }

        public int DoadorId { get; set; }
        public virtual Doador Doador { get; set; }

        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
