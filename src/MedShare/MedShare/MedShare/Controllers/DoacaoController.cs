using System.Diagnostics;
using MedShare.Models;
using MedShare.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            //if (!ModelState.IsValid)
            //    return View(doacao);

     

            try
            {
                var usuarioId = User.Identity?.Name ?? "anonimo";

                doacao.Status = "Disponível";
                doacao.DataCriacao = DateTime.Now;

                //if (doacao.ValidadeDoacao == default || doacao.ValidadeDoacao.Year < 2000)
                //    doacao.ValidadeDoacao = DateTime.Now.AddMonths(6);

                //if (doacao.PrazoAnalise == default)
                //    doacao.PrazoAnalise = DateTime.Now.AddDays(1);

                await _doacaoService.CadastrarAsync(doacao, usuarioId);

                TempData["Sucesso"] = "Doação cadastrada com sucesso!";
                return RedirectToAction(nameof(MinhasDoacoes));
            }
            catch (ArgumentException ex)
            {
                TempData["Erro"] = ex.Message;
                return View(doacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar doação");
                ModelState.AddModelError(string.Empty, "Erro ao cadastrar doação. Tente novamente.");
                return View(doacao);
            }
        }


        [HttpGet("doador/minhas")]
        public async Task<IActionResult> MinhasDoacoes()
        {
            var usuarioId = User.Identity?.Name ?? "anonimo";
            var doacoes = await _doacaoService.ListarPorUsuarioAsync(usuarioId);
            return View("MinhasDoacoes", doacoes);
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
