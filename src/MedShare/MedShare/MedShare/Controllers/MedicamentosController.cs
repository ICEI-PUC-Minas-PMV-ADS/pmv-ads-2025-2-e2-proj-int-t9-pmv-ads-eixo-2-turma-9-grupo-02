using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MedShare.Controllers
{
    [Authorize(Roles="Instituicao")] // Restringe acesso: somente usuários com perfil de Instituição.
    public class MedicamentosController : Controller
    {
        private readonly AppDbContext _context;
        public MedicamentosController(AppDbContext context)
        {
            _context = context;
        }

        // Lista todos os registros de estoque (InstituicaoMedicamento) da instituição logada.
        public async Task<IActionResult> Index()
        {
            var instituicaoEmail = User.Claims.FirstOrDefault(c => c.Type == "InstituicaoEmail")?.Value;
            if (instituicaoEmail == null) return Unauthorized();
            var inst = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoEmail == instituicaoEmail);
            if (inst == null) return NotFound();

            var lista = await _context.InstituicaoMedicamentos
                .Include(im => im.Medicamento) // Inclui dados do medicamento (nome)
                .Where(im => im.InstituicaoId == inst.InstituicaoId)
                .ToListAsync();
            return View(lista);
        }

        // Lista apenas medicamentos cuja quantidade em estoque é menor que 11 caixas (escassez).
        public async Task<IActionResult> Escassez()
        {
            var instituicaoEmail = User.Claims.FirstOrDefault(c => c.Type == "InstituicaoEmail")?.Value;
            if (instituicaoEmail == null) return Unauthorized();
            var inst = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoEmail == instituicaoEmail);
            if (inst == null) return NotFound();

            var criticos = await _context.InstituicaoMedicamentos
                .Include(im => im.Medicamento)
                .Where(im => im.InstituicaoId == inst.InstituicaoId && im.QuantidadeCaixas < 11) // Regra de escassez
                .OrderBy(im => im.QuantidadeCaixas) // Ordena do menor para o maior para facilitar visualização
                .ToListAsync();
            return View(criticos);
        }

        // Método utilitário (não exposto como rota) para criar estoque inicial zerado para nova instituição.
        [NonAction]
        public async Task CriarBaseParaInstituicao(int instituicaoId)
        {
            var jaExiste = await _context.InstituicaoMedicamentos.AnyAsync(im => im.InstituicaoId == instituicaoId);
            if (jaExiste) return; // Evita duplicar registros caso já tenha sido inicializado.
            var todos = await _context.Medicamentos.ToListAsync(); // Pega catálogo global
            foreach (var med in todos)
            {
                _context.InstituicaoMedicamentos.Add(new InstituicaoMedicamento
                {
                    InstituicaoId = instituicaoId,
                    MedicamentoId = med.MedicamentoId,
                    QuantidadeCaixas = 0 // Estoque inicia vazio até atualização manual
                });
            }
            await _context.SaveChangesAsync();
        }

        // Exibe tela para editar quantidade de caixas de um medicamento específico no estoque da instituição.
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.InstituicaoMedicamentos.Include(i=>i.Medicamento).FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Recebe nova quantidade e atualiza o registro em banco.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int quantidadeCaixas)
        {
            var item = await _context.InstituicaoMedicamentos.FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            item.QuantidadeCaixas = quantidadeCaixas < 0 ? 0 : quantidadeCaixas; // Sanitiza valor negativo
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Mostra detalhes de um item de estoque (inclui nome do medicamento e quantidade atual).
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.InstituicaoMedicamentos.Include(i=>i.Medicamento).FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Tela de confirmação para remover vínculo do medicamento com a instituição (não remove do catálogo global).
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.InstituicaoMedicamentos.Include(i=>i.Medicamento).FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            return View(item);
        }

        // Remove o registro de estoque da instituição para o medicamento escolhido.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.InstituicaoMedicamentos.FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            _context.InstituicaoMedicamentos.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
