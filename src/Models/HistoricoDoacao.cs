namespace MedShare.Models {
    public class HistoricoDoacao 
    {
        public int HistoricoDoacaoId { get; set; }
        public DateTime HistoricoDoacaoDataFinalizacao { get; set; }
        public String HistoricoDoacaoResultado { get; set; }
        public int DoacaoId { get; set; }
        public virtual Doacao Doacao { get; set; }
    }
}
