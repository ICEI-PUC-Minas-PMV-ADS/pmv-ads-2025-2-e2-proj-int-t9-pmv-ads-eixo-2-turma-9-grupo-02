using System.Diagnostics;
using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Controllers
{
    // Controller responsável pelas ações administrativas do sistema.
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin (redirects to Dashboard)
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        // GET: Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalUsuarios = await _context.Doadores.CountAsync() + await _context.Instituicoes.CountAsync();
            /*var totalDoacoes = await _context.Doacoes.CountAsync();*/
            var totalDoadores = await _context.Doadores.CountAsync();
            var totalInstituicoes = await _context.Instituicoes.CountAsync();
            /*var doacoesAtivas = await _context.Doacoes.Where(d => d.Validade > DateTime.Now).CountAsync();*/

            ViewBag.TotalUsuarios = totalUsuarios;
            /*ViewBag.TotalDoacoes = totalDoacoes;*/
            ViewBag.TotalDoadores = totalDoadores;
            ViewBag.TotalInstituicoes = totalInstituicoes;
            /* ViewBag.DoacoesAtivas = doacoesAtivas;*/

            return View();
        }

        // GET: Admin/Notificacoes
        public IActionResult Notificacoes()
        {
            // TODO: Implementar lógica de notificações
            return View();
        }

        // GET: Admin/Chat
        public IActionResult Chat()
        {
            // TODO: Implementar lógica de chat
            return View();
        }

        // GET: Admin/Relatorio
        public async Task<IActionResult> Relatorio()
        {
            /*var totalDoacoes = await _context.Doacoes.CountAsync();*/
            var totalDoadores = await _context.Doadores.CountAsync();
            var totalInstituicoes = await _context.Instituicoes.CountAsync();

            /* ViewBag.TotalDoacoes = totalDoacoes;*/
            ViewBag.TotalDoadores = totalDoadores;
            ViewBag.TotalInstituicoes = totalInstituicoes;

            return View();
        }

        // GET: Admin/EditarPerfil
        public IActionResult EditarPerfil()
        {
            // TODO: Implementar edição de perfil
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Admin/EditarPerfil
        public IActionResult EditarPerfil(object perfil)
        {
            // TODO: Implementar lógica de edição
            if (ModelState.IsValid)
            {
                TempData["Sucesso"] = "Perfil atualizado com sucesso!";
                return RedirectToAction("EditarPerfil");
            }
            return View(perfil);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}