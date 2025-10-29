using MedShare.Models;

namespace MedShare.Services
{
    public interface IDoacaoService
    {
        Task<Doacao> CadastrarAsync(Doacao doacao, string usuarioId);
        Task<Doacao?> ObterPorIdAsync(int id);
        Task<List<Doacao>> ListarTodasAsync();
        Task<List<Doacao>> ListarPorUsuarioAsync(string usuarioId);
        Task<Doacao> AtualizarAsync(Doacao doacao);
        Task RemoverAsync(int id);
    }
}
