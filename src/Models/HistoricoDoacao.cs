using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedShare.Models {

    [Table("HistoricoDoacaos")]
    public class HistoricoDoacao 
    {
        [Key]
        public int HistoricoDoacaoId { get; set; }
        public DateTime HistoricoDoacaoDataFinalizacao { get; set; }
        public String HistoricoDoacaoResultado { get; set; }
        public int DoacaoId { get; set; }
        public virtual Doacao Doacao { get; set; }
    }
}
