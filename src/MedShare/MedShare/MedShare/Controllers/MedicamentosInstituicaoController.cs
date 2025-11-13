using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using System.Security.Claims;

namespace MedShare.Controllers
{
    [Authorize(Roles = "Instituicao")]
    public class MedicamentosInstituicaoController : Controller
    {
        private readonly AppDbContext _context;
        public MedicamentosInstituicaoController(AppDbContext context)
        {
            _context = context;
        }

        // Lista todos os medicamentos da instituição logada
        public async Task<IActionResult> Index()
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var lista = await _context.MedicamentosInstituicao.Where(m => m.InstituicaoId == instId).ToListAsync();
            return View(lista);
        }

        // Lista somente medicamentos em escassez crítica (< 11 caixas)
        public async Task<IActionResult> Escassez()
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var lista = await _context.MedicamentosInstituicao.Where(m => m.InstituicaoId == instId && m.QuantidadeCaixas < 11).ToListAsync();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View(new MedicamentoInstituicao());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicamentoInstituicao model)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            ModelState.Remove(nameof(model.Instituicao));
            if (ModelState.IsValid)
            {
                model.InstituicaoId = instId;
                _context.MedicamentosInstituicao.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var med = await _context.MedicamentosInstituicao.FirstOrDefaultAsync(m => m.Id == id && m.InstituicaoId == instId);
            if (med == null) return NotFound();
            return View(med);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicamentoInstituicao model)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            if (id != model.Id) return NotFound();
            var med = await _context.MedicamentosInstituicao.FirstOrDefaultAsync(m => m.Id == id && m.InstituicaoId == instId);
            if (med == null) return NotFound();
            ModelState.Remove(nameof(model.Instituicao));
            if (ModelState.IsValid)
            {
                med.Nome = model.Nome;
                med.QuantidadeCaixas = model.QuantidadeCaixas;
                med.Observacao = model.Observacao;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var med = await _context.MedicamentosInstituicao.Include(m => m.Instituicao).FirstOrDefaultAsync(m => m.Id == id && m.InstituicaoId == instId);
            if (med == null) return NotFound();
            return View(med);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var med = await _context.MedicamentosInstituicao.FirstOrDefaultAsync(m => m.Id == id && m.InstituicaoId == instId);
            if (med == null) return NotFound();
            return View(med);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(instIdStr, out var instId)) return Unauthorized();
            var med = await _context.MedicamentosInstituicao.FirstOrDefaultAsync(m => m.Id == id && m.InstituicaoId == instId);
            if (med == null) return NotFound();
            _context.MedicamentosInstituicao.Remove(med);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
