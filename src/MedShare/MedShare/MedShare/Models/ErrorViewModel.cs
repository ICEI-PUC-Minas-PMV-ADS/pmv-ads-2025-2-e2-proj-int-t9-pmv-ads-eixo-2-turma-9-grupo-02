namespace MedShare.Models
{
// Modelo para exibir informações de erro na aplicação.
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
