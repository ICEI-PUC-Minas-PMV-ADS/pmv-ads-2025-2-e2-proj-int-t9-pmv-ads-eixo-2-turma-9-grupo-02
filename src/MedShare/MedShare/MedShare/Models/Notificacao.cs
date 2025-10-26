namespace MedShare.Models
{
    public class Notificacao
    {
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public bool Lida { get; set; }
        public DateTime Horario { get; set; }
    }
}
