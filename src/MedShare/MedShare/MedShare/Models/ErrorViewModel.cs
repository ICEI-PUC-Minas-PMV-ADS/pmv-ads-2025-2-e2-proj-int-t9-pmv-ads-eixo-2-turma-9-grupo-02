namespace MedShare.Models
{
// Modelo para exibir informa��es de erro na aplica��o.
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
