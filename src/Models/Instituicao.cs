namespace MedShare.Models {
    public class Instituicao 
    {
        public int InstituicaoId { get; set; }
        public String InstituicaoNome { get; set; }
        public String InstituicaoCnpj { get; set; }
        public String InstituicaoEmail { get; set; }
        public String InstituicaoEndereco { get; set; }
        public String InstituicaoSenha { get; set; }
        public List<Doacao> Doacaos { get; set; }

    }
}
