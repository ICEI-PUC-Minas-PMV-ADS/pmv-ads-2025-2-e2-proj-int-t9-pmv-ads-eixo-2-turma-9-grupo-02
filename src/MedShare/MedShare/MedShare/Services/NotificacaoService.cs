using MedShare.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedShare.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly AppDbContext _context;

        public NotificacaoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CriarNotificacaoAsync(int doadorId, string mensagem)
        {
            var notificacao = new Notificacao
            {
                DoadorId = doadorId,
                Mensagem = mensagem
            };
            _context.Notificacoes.Add(notificacao);
            await _context.SaveChangesAsync();
        }
    }
}
