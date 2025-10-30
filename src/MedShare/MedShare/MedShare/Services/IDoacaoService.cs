using MedShare.Models;

namespace MedShare.Services
{
    public interface IDoacaoService
    {
        Task<Doacao> CadastrarAsync(Doacao doacao);
        Task<Doacao?> ObterPorIdAsync(int id);
        Task<List<Doacao>> ListarTodasAsync();
        Task<List<Doacao>> ListarPorUsuarioAsync(string usuarioEmail);
        Task<Doacao> AtualizarAsync(Doacao doacao);
        Task RemoverAsync(int id);
    }
}
