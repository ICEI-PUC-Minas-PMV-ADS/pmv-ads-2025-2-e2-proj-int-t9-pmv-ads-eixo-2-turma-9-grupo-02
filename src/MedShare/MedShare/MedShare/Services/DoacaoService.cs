using MedShare.Models;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DoacaoService> _logger;

        public DoacaoService(AppDbContext context, IWebHostEnvironment env, ILogger<DoacaoService> logger)
        {
            _context = context;
            _env = env;
            _logger = logger;
        }

        public async Task<Doacao> CadastrarAsync(Doacao doacao, string usuarioEmail)
        {
            // if (doacao.FotoDoacao == null)
                // throw new ArgumentException("É obrigatório enviar uma foto da doação.");
            // if (doacao.ReceitaDoacao == null)
                // throw new ArgumentException("É obrigatório enviar a receita médica da doação.");
                // Console.WriteLine("ReceitaDoacao recebida: " + doacao.ReceitaDoacao.FileName);

            // Busca o doador correspondente ao e-mail (ou ID, se preferir)
            var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.DoadorEmail == "doadortestenia@gmail.com");
            if (doador == null)
                throw new InvalidOperationException("Doador não encontrado para o usuário logado.");
                Console.WriteLine("Doador encontrado: " + doador.DoadorEmail);

            // Relaciona a doação ao objeto Doador
            doacao.Doador = doador;
            doacao.DoadorID = doador.DoadorId;
            doacao.Status = "Disponível";
            doacao.DataCriacao = DateTime.Now;
            doacao.PrazoAnalise = DateTime.Now.AddHours(48);

            // Salvar arquivos (foto e receita)
            //string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            //Directory.CreateDirectory(uploadsFolder);

            //string fotoFileName = Guid.NewGuid() + Path.GetExtension(doacao.FotoDoacao.FileName);
            //string fotoPath = Path.Combine(uploadsFolder, fotoFileName);
            //using (var stream = new FileStream(fotoPath, FileMode.Create))
            //    await doacao.FotoDoacao.CopyToAsync(stream);
            //doacao.CaminhoFoto = "/uploads/" + fotoFileName;

            //string receitaFileName = Guid.NewGuid() + Path.GetExtension(doacao.ReceitaDoacao.FileName);
            //string receitaPath = Path.Combine(uploadsFolder, receitaFileName);
            //using (var stream = new FileStream(receitaPath, FileMode.Create))
            //    await doacao.ReceitaDoacao.CopyToAsync(stream);
            //doacao.CaminhoReceita = "/uploads/" + receitaFileName;

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Doação {Id} criada por {Doador}", doacao.Id, doador.DoadorEmail);

            return doacao;
        }

        public async Task<Doacao?> ObterPorIdAsync(int id) =>
            await _context.Doacoes
                .Include(d => d.Doador)
                .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<List<Doacao>> ListarTodasAsync() =>
            await _context.Doacoes
                .Include(d => d.Doador)
                .AsNoTracking()
                .ToListAsync();

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


