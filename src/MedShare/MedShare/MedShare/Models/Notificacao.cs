namespace MedShare.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public bool Lida { get; set; }
        public DateTime Horario { get; set; }
        public string Tipo { get; set; } // "sucesso", "erro", "aviso", "informação"
    }
}
