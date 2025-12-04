using MedShare.Models;
using System.Threading.Tasks;

namespace MedShare.Services
{
    public interface INotificacaoService
    {
        Task CriarNotificacaoAsync(int doadorId, string mensagem);
    }
}
