namespace MedShare.Models {
    public class Doador 
    {
        public int DoadorId { get; set; }
        public string DoadorName { get; set; }
        public string DoadorCpf { get; set; }
        public string DoadorEmail { get; set; }
        public string DoadorSenha { get; set; }
        public DateTime DoadorDataNascimento { get; set; }
        public List<Doacao> Doacaos { get; set; } 
    }
}
