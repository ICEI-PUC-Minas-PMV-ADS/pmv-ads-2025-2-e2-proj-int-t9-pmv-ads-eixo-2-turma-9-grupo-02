using System.Diagnostics;
using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MedShare.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly INotificacaoService _notificacaoService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, INotificacaoService notificacaoService)
        {
            _logger = logger;
            _context = context;
            _notificacaoService = notificacaoService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (User.IsInRole("Doador"))
                return RedirectToAction("HomePageDoador", "Home");
            else if (User.IsInRole("Instituicao"))
                return RedirectToAction("HomePageInstituicao", "Home");
            else if (User.IsInRole("Admin"))
                return RedirectToAction("HomePageAdmin", "Home");
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        public IActionResult Doar()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> HomePageDoador()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (!User.IsInRole("Doador"))
                return RedirectToAction("HomePageInstituicao", "Home");

            var doadorIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(doadorIdStr, out int doadorId))
                return RedirectToAction("Login", "Auth");

            var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.DoadorId == doadorId);
            int doacoesFinalizadas = 0;
            int doacoesPendentes = 0;
            int instituicoesComEstoqueCritico = 0;
            string debugInfo = string.Empty;
            if (doador != null)
            {
                debugInfo += $"DoadorId: {doador.DoadorId}, Email: {doador.DoadorEmail}\n";
                // Contagem direta das doações pendentes e finalizadas no banco de dados
                doacoesPendentes = await _context.Doacoes.CountAsync(x => x.DoadorId == doadorId && x.Status == StatusDoacao.Pendente);
                // Considera status Finalizado e Aprovado como finalizados
                doacoesFinalizadas = await _context.Doacoes.CountAsync(x => x.DoadorId == doadorId && (x.Status == StatusDoacao.Finalizado || x.Status == StatusDoacao.Aprovado));
                debugInfo += $"Pendentes (direto no banco): {doacoesPendentes}\n";
                debugInfo += $"Finalizadas (direto no banco): {doacoesFinalizadas}\n";
                // Para o card de estoque crítico, mantém a lógica anterior
                var doacoes = await _context.Doacoes
                    .Where(x => x.DoadorId == doadorId)
                    .ToListAsync();
                var instituicoesIds = doacoes
                    .Where(x => x.InstituicaoId.HasValue)
                    .Select(x => x.InstituicaoId.Value)
                    .Distinct()
                    .ToList();
                var estoques = (await _context.EstoqueMedicamentos
                    .Where(e => instituicoesIds.Contains(e.InstituicaoId) && e.Quantidade.HasValue)
                    .ToListAsync())
                    .Where(e => e.Quantidade.Value <= e.QuantidadeMinima)
                    .ToList();
                instituicoesComEstoqueCritico = estoques.Select(e => e.InstituicaoId).Distinct().Count();
                debugInfo += $"Instituições com estoque crítico: {instituicoesComEstoqueCritico}\n";
            }
            ViewBag.DoacoesFinalizadas = doacoesFinalizadas;
            ViewBag.DoacoesPendentes = doacoesPendentes;
            ViewBag.EstoquesCriticos = instituicoesComEstoqueCritico;
            ViewBag.DebugInfo = debugInfo;
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> HomePageInstituicao()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (!User.IsInRole("Instituicao"))
                return RedirectToAction("HomePageDoador", "Home");

            // Pega o id da instituição logada
            var instituicaoIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(instituicaoIdStr, out int instituicaoId))
                return RedirectToAction("Login", "Auth");

            // Card 1: Total de doações finalizadas para esta instituição
            int totalDoacoesFinalizadas = await _context.Doacoes.CountAsync(d => d.InstituicaoId == instituicaoId && d.Status == StatusDoacao.Finalizado);

            // Card 2: Medicamentos em estoque crítico (nome e quantidade)
            var todosEstoque = await _context.EstoqueMedicamentos
                .Where(e => e.InstituicaoId == instituicaoId && e.Quantidade.HasValue)
                .ToListAsync();
            var criticos = todosEstoque
                .Where(e => e.Quantidade.Value <= e.QuantidadeMinima)
                .ToList();

            ViewBag.TotalDoacoesFinalizadas = totalDoacoesFinalizadas;
            ViewBag.EstoquesCriticos = criticos;
            // Card 3: Não alterar ainda
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> HomePageAdmin()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");
            return RedirectToAction("AdminUsuarios");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Roles = "Doador")]
        public async Task<IActionResult> Notificacoes()
        {
            var doadorIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(doadorIdStr, out int doadorId))
                return Unauthorized();
            var notificacao = await _context.Notificacoes
                .Where(n => n.DoadorId == doadorId)
                .OrderByDescending(n => n.DataCriacao)
                .FirstOrDefaultAsync();
            return PartialView("_Notificacoes", notificacao == null ? new List<Notificacao>() : new List<Notificacao> { notificacao });
        }

        [Authorize(Roles = "Doador")]
        public async Task<IActionResult> HistoricoNotificacoes()
        {
            var doadorIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(doadorIdStr, out int doadorId))
                return Unauthorized();
            var notificacoes = await _context.Notificacoes
                .Where(n => n.DoadorId == doadorId)
                .OrderByDescending(n => n.DataCriacao)
                .ToListAsync();
            // Marca todas como lidas
            foreach (var n in notificacoes.Where(n => !n.Lida))
                n.Lida = true;
            await _context.SaveChangesAsync();
            return View("HistoricoNotificacoes", notificacoes);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUsuarios()
        {
            var doadores = await _context.Doadores.ToListAsync();
            var instituicoes = await _context.Instituicoes.ToListAsync();
            // Card 1: Doações finalizadas
            int totalFinalizadas = await _context.Doacoes.CountAsync(d => d.Status == StatusDoacao.Finalizado);
            // Card 2: Estoques críticos
            int totalCriticos = await _context.EstoqueMedicamentos.CountAsync(e => e.Quantidade.HasValue && e.Quantidade.Value <= e.QuantidadeMinima);
            // Card 3: Doações pendentes
            int totalPendentes = await _context.Doacoes.CountAsync(d => d.Status == StatusDoacao.Pendente);
            ViewBag.Doadores = doadores;
            ViewBag.Instituicoes = instituicoes;
            ViewBag.TotalFinalizadas = totalFinalizadas;
            ViewBag.TotalCriticos = totalCriticos;
            ViewBag.TotalPendentes = totalPendentes;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DesativarUsuario(int id, string tipo)
        {
            if (tipo == "Doador")
            {
                var doador = await _context.Doadores.FindAsync(id);
                if (doador != null)
                {
                    doador.Ativo = false;
                    await _context.SaveChangesAsync();
                }
            }
            else if (tipo == "Instituicao")
            {
                var instituicao = await _context.Instituicoes.FindAsync(id);
                if (instituicao != null)
                {
                    instituicao.Ativo = false;
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("AdminUsuarios");
        }
    }
}

// Controller responsável pelas páginas principais e navegação do sistema.
