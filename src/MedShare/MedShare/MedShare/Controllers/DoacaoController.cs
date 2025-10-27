using System.Diagnostics;
using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Controllers
{
    [Authorize]
    public class DoacaoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DoacaoController> _logger;

        public DoacaoController(AppDbContext context, ILogger<DoacaoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Doacao/Doar
        public IActionResult Doar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Doacao/Doar
        public async Task<IActionResult> Doar(Doacao doacao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    doacao.Status = "Disponível";
                    doacao.DataCriacao = DateTime.Now;
                    doacao.PrazoAnalise = DateTime.Now.AddHours(48);
                    _context.Doacoes.Add(doacao);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Doação cadastrada com sucesso!";
                    return RedirectToAction("MinhasDoacoes");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar doação");
                    TempData["Erro"] = "Erro ao cadastrar doação. Tente novamente.";
                }
            }
            return View(doacao);
        }

        // GET: Doacao/MinhasDoacoes
        public async Task<IActionResult> MinhasDoacoes()
        {
            // TODO: Filtrar pelas doações do usuário logado
            var doacoes = await _context.Doacoes.ToListAsync();
            return View(doacoes);
        }

        // GET: Doacao/BuscarInstituicoes
        public async Task<IActionResult> BuscarInstituicoes()
        {
            var instituicoes = await _context.Instituicoes.ToListAsync();
            return View(instituicoes);
        }

        // GET: Doacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doacao = await _context.Doacoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doacao == null)
            {
                return NotFound();
            }

            return View(doacao);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}