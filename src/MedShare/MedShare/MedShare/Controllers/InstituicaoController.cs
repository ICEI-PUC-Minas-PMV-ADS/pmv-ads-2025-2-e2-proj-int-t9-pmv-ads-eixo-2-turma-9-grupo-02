using MedShare.Models;
using MedShare.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Controllers
{
    public class InstituicaoController : Controller
    {
        private readonly AppDbContext _context;

        public InstituicaoController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult HomePagePJ()
        {
            var totalMedicamentosRecebidos = _context.Doacoes
                .Count(d => d.Status == "Aprovado");

            var doacoesAguardandoAprovacao = _context.Doacoes
                .Count(d => d.Status == "Aguardando Instituição Aprovar");

            var totalSolicitacoesPendentes = _context.Doacoes
                .Count(d => d.Status == "Aguardando Doador Aprovar");

            var viewModel = new HomePagePJViewModel
            {
                TotalMedicamentosRecebidos = totalMedicamentosRecebidos,
                DoacoesAguardandoAprovacao = doacoesAguardandoAprovacao,
                TotalSolicitacoesPendentes = totalSolicitacoesPendentes
            };

            return View(viewModel);
        }

        // GET: Rede de Doações
        public IActionResult Rede()
        {
            var doacoes = _context.Doacoes
                .Include(d => d.Doador)
                .Where(d => d.Status == "Disponível")
                .OrderByDescending(d => d.DataCriacao)
                .ToList();
            Console.WriteLine(doacoes.Count);
            return View(doacoes);
        }
        // GET: Detalhes da Doação
        public IActionResult DetalhesDoacao(int id)
        {
            var doacao = _context.Doacoes
                .Include(d => d.Doador)
                .FirstOrDefault(d => d.Id == id);

            if (doacao == null)
                return NotFound();

            return View(doacao);
        }

        // 🔹Detalhar doação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitarDoacao(int id)
        {
            var doacao = await _context.Doacoes.FirstOrDefaultAsync(d => d.Id == id);
            if (doacao == null)
                return NotFound();

            doacao.Status = "Aguardando Doador Aprovar";

            await _context.SaveChangesAsync();
            TempData["Mensagem"] = "Solicitação enviada! Aguarde a resposta do doador.";


            return RedirectToAction("Rede");
        }

        // Minhas Doações PJ
        public IActionResult MinhasDoacoesPJ()
        {
            // Busca todas as doações que já foram solicitadas ou recebidas pela instituição
            var doacoes = _context.Doacoes
                .Include(d => d.Doador)
                .Where(d => d.Status != "Disponível")
                //.Where(d => d.InstituicaoId == instituicaoLogadaId)
                .OrderByDescending(d => d.DataCriacao)
                .ToList();

            return View(doacoes);
        }
    }

}
