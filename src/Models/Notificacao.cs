namespace MedShare.Models {
    public class Notificacao 
    {
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
