namespace MedShare.Models {
    public class Doacao 
    {
        public int DoacaoId { get; set; }
        public string DoacaoNomeMedicamento { get; set; }
        public DateTime DoacaoDataValidade { get; set; }
        public int DoacaoQuantidadeMedicamento { get; set; }
        public int DoacaoUrlReceita { get; set; }
        public int DoacaoUrlFoto { get; set; }
        public DoacaoStatus Status { get; set; }
        /* O certo para manter o padrão de nomes teria que ser public DoacaoStatus DoacaoStatus { get; set; } */

        public int DoadorId { get; set; }
        public virtual Doador Doador { get; set; }
        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
