using MedShare.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MedShare.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DoacaoService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoacaoService(AppDbContext context, IWebHostEnvironment env, ILogger<DoacaoService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _env = env;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Doacao> CadastrarAsync(Doacao doacao)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var doadorIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doadorIdClaim))
                throw new InvalidOperationException("ID do doador não encontrado no contexto de autenticação.");

            int doadorId = int.Parse(doadorIdClaim);
            var doador = await _context.Doadores.FindAsync(doadorId);

            if (doador == null)
                throw new InvalidOperationException($"Doador com ID {doadorId} não encontrado.");

            _logger.LogInformation("Doador autenticado: {Id} - {Email}", doador.DoadorId, doador.DoadorEmail);

            if (doacao.FotoDoacao == null)
                throw new ArgumentException("É obrigatório enviar uma foto da doação.");
            if (doacao.ReceitaDoacao == null)
                throw new ArgumentException("É obrigatório enviar a receita médica da doação.");

            doacao.Doador = doador;
            doacao.DoadorID = doador.DoadorId;
            doacao.Status = "Disponível";
            doacao.DataCriacao = DateTime.Now;
            doacao.PrazoAnalise = DateTime.Now.AddHours(48);

            string uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
            Directory.CreateDirectory(uploadsFolder);

            string fotoFileName = Guid.NewGuid() + Path.GetExtension(doacao.FotoDoacao.FileName);
            string fotoPath = Path.Combine(uploadsFolder, fotoFileName);
            using (var stream = new FileStream(fotoPath, FileMode.Create))
                await doacao.FotoDoacao.CopyToAsync(stream);
            doacao.CaminhoFoto = "/uploads/" + fotoFileName;

            string receitaFileName = Guid.NewGuid() + Path.GetExtension(doacao.ReceitaDoacao.FileName);
            string receitaPath = Path.Combine(uploadsFolder, receitaFileName);
            using (var stream = new FileStream(receitaPath, FileMode.Create))
                await doacao.ReceitaDoacao.CopyToAsync(stream);
            doacao.CaminhoReceita = "/uploads/" + receitaFileName;

            _logger.LogInformation("Arquivos salvos: {foto}, {receita}", doacao.CaminhoFoto, doacao.CaminhoReceita);

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Doação {Id} criada por doador {DoadorId}", doacao.Id, doador.DoadorId);
            return doacao;
        }
        public async Task<Doacao?> ObterPorIdAsync(int id) => 
            await _context.Doacoes
            .Include(d => d.Doador)
            .FirstOrDefaultAsync(d => d.Id == id); 
        public async Task<List<Doacao>> ListarTodasAsync() => 
            await _context.Doacoes
            .Include(d => d.Doador)
            .AsNoTracking().ToListAsync(); 
        public async Task<List<Doacao>> ListarPorUsuarioAsync(string usuarioEmail) => 
            await _context.Doacoes
            .Include(d => d.Doador)
            .Where(d => d.Doador != null && d.Doador.DoadorEmail == usuarioEmail)
            .AsNoTracking()
            .ToListAsync(); 
        public async Task<Doacao> AtualizarAsync(Doacao doacao) 
        { 
            _context.Doacoes.Update(doacao); 
            await _context.SaveChangesAsync(); 
            return doacao; 
        }
        public async Task RemoverAsync(int id) 
        { 
            var d = await _context.Doacoes.FindAsync(id); 
            if (d == null) return; 
            _context.Doacoes.Remove(d); 
            await _context.SaveChangesAsync(); 
        }
    }
}
