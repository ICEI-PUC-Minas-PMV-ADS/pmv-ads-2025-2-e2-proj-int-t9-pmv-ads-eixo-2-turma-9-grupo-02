using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using System.Security.Claims;

namespace MedShare.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class EstoqueMedicamentosController : Controller
    {
        private readonly AppDbContext _context;
        public EstoqueMedicamentosController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Pega o ID da instituição logada
            var instituicaoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(instituicaoId, out int id))
                return Unauthorized();

            // Filtra o estoque da instituição logada
            var dados = await _context.EstoqueMedicamentos
                .Where(e => e.InstituicaoId == id)
                .ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View(new EstoqueMedicamento());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstoqueMedicamento estoque)
        {
            var instituicaoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(instituicaoId, out int id))
            {
                ModelState.AddModelError("", "Erro ao identificar instituição logada.");
                return View(estoque);
            }

            estoque.InstituicaoId = id;

            if (string.IsNullOrWhiteSpace(estoque.NomeMedicamento))
                ModelState.AddModelError("NomeMedicamento", "Obrigatório informar o nome do medicamento!");

            if (estoque.Validade == null)
                ModelState.AddModelError("Validade", "Obrigatório informar a validade!");

            if (estoque.Quantidade == null || estoque.Quantidade <= 0)
                ModelState.AddModelError("Quantidade", "A quantidade deve ser maior que zero!");

            if (!ModelState.IsValid)
                return View(estoque);

            // Salva no banco
            _context.Add(estoque);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.EstoqueMedicamentos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EstoqueMedicamento estoque)
        {
            if (id != estoque.Id)
                return NotFound();

            var existente = await _context.EstoqueMedicamentos.FindAsync(id);
            if (existente == null)
                return NotFound();

            // Validações simples
            if (string.IsNullOrWhiteSpace(estoque.NomeMedicamento))
                ModelState.AddModelError("NomeMedicamento", "Obrigatório informar o nome do medicamento!");

            if (estoque.Validade == null)
                ModelState.AddModelError("Validade", "Obrigatório informar a validade!");

            if (estoque.Quantidade == null || estoque.Quantidade <= 0)
                ModelState.AddModelError("Quantidade", "A quantidade deve ser maior que zero!");

            if (!ModelState.IsValid)
            {
                ViewBag.Instituicoes = _context.Instituicoes.ToList();
                return View(estoque);
            }

            // Atualiza somente campos permitidos
            existente.NomeMedicamento = estoque.NomeMedicamento;
            existente.Validade = estoque.Validade;
            existente.Quantidade = estoque.Quantidade;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.EstoqueMedicamentos
                .Include(e => e.Instituicao)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.EstoqueMedicamentos
                .Include(e => e.Instituicao)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.EstoqueMedicamentos.FindAsync(id);
            if (dados == null)
                return NotFound();

            _context.EstoqueMedicamentos.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> BuscarInstituicoes(string busca)
        {
            // 1. Busca instituições (com ou sem filtro por cidade)
            var instituicoesQuery = _context.Instituicoes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var lower = busca.Trim().ToLower();
                instituicoesQuery = instituicoesQuery
                    .Where(i => i.InstituicaoEndereco.ToLower().Contains(lower));
            }

            var instituicoes = await instituicoesQuery.ToListAsync();

            // 2. Carrega TODOS os estoques (não dá para filtrar por quantidade mínima via SQL)
            var todosEstoques = await _context.EstoqueMedicamentos.ToListAsync();

            // 3. Filtra os críticos em memória usando QuantidadeMinima
            var criticosPorInstituicao = todosEstoques
                .Where(e => e.Quantidade.HasValue && e.Quantidade.Value <= e.QuantidadeMinima) // <= 15
                .GroupBy(e => e.InstituicaoId)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.CriticosNomes = criticosPorInstituicao;

            return View(instituicoes);
        }
    }
}
