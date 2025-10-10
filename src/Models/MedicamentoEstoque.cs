namespace MedShare.Models {
    public class MedicamentoEstoque 
    {
        public int MedicamentoEstoqueId { get; set; }
        public String MedicamentoEstoqueNome { get; set; }
        public int MedicamentoEstoqueQuantidade { get; set; }
        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
