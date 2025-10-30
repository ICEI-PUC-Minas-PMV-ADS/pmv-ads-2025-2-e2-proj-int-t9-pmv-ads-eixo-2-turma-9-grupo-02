using MedShare.Models;
using MedShare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace MedShare.Controllers
{
    [Authorize]
    [Route("Doacao")] // base route /Doacoes
    public class DoacaoController : Controller
    {
        private readonly IDoacaoService _doacaoService;
        private readonly ILogger<DoacaoController> _logger;
        private readonly AppDbContext _context;

        public DoacaoController(
            IDoacaoService doacaoService,
            ILogger<DoacaoController> logger,
            AppDbContext context)
        {
            _doacaoService = doacaoService;
            _logger = logger;
            _context = context;
        }

        //  DOADORusuário comum

        [HttpGet("doador/doar")]
        public IActionResult Doar() => View();

        [HttpPost("doador/doar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Doar(Doacao doacao)
        {
            _logger.LogInformation(">>> FotoDoacao: {foto}, ReceitaDoacao: {receita}",
                doacao.FotoDoacao?.FileName, doacao.ReceitaDoacao?.FileName);


            try
            {
                if (!ModelState.IsValid)
                {
                    // Log 
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                        _logger.LogError("Erro de model binding: {ErrorMessage}", error.ErrorMessage);

                    _logger.LogWarning("ModelState inválido. Reexibindo o formulário.");
                    return View(doacao);
                }

                var usuarioEmail = User.Identity?.Name ?? "anonimo";
                _logger.LogInformation("Usuário autenticado: {usuario}", usuarioEmail);

                doacao.Status = "Disponível";
                doacao.DataCriacao = DateTime.Now;
                doacao.PrazoAnalise = DateTime.Now.AddHours(48);

                _logger.LogInformation("Chamando CadastrarAsync...");
                await _doacaoService.CadastrarAsync(doacao);

                _logger.LogInformation("Doação salva com sucesso.");
                TempData["Sucesso"] = "Doação cadastrada com sucesso!";
                return RedirectToAction(nameof(MinhasDoacoes));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro de validação: {mensagem}", ex.Message);
                TempData["Erro"] = ex.Message;
                return View(doacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar doação: {Mensagem}", ex.Message);
                ModelState.AddModelError(string.Empty, $"Erro interno: {ex.Message}");
                return View(doacao);
            }

        }


        [HttpGet("doador/minhas")]
        public async Task<IActionResult> MinhasDoacoes()
        {
            var doadorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doadorIdClaim))
                return Unauthorized();

            int doadorId = int.Parse(doadorIdClaim);

            var doacoes = await _context.Doacoes
                .Include(d => d.Doador)
                .Where(d => d.DoadorID == doadorId)
                .OrderByDescending(d => d.DataCriacao)
                .ToListAsync();

            return View(doacoes);
        }

        [HttpGet("doador/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var doacao = await _doacaoService.ObterPorIdAsync(id);
            if (doacao == null) return NotFound();
            return View("Details", doacao);
        }

        //  ADMINISTRADOR

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/index")]
        public async Task<IActionResult> Index()
        {
            var todas = await _doacaoService.ListarTodasAsync();
            return View("Index", todas);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var doacao = await _doacaoService.ObterPorIdAsync(id);
            if (doacao == null) return NotFound();
            return View("Edit", doacao);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doacao doacao)
        {
            if (id != doacao.Id) return NotFound();
            if (!ModelState.IsValid) return View("Edit", doacao);

            try
            {
                await _doacaoService.AtualizarAsync(doacao);
                TempData["Sucesso"] = "Doação atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar doação");
                TempData["Erro"] = "Erro ao atualizar doação.";
                return View("Edit", doacao);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var doacao = await _doacaoService.ObterPorIdAsync(id);
            if (doacao == null) return NotFound();
            return View("Delete", doacao);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doacaoService.RemoverAsync(id);
            TempData["Sucesso"] = "Doação removida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
