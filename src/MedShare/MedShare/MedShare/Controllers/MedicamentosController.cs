<<<<<<< Updated upstream
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
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using System.Security.Claims;

namespace MedShare.Controllers
{
    [Authorize]
    public class MedicamentosController : Controller
    {
        private readonly AppDbContext _context;

>>>>>>> Stashed changes
        public MedicamentosController(AppDbContext context)
        {
            _context = context;
        }

<<<<<<< Updated upstream
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
=======
        // GET: Medicamentos
        public async Task<IActionResult> Index()
        {
            // Verificar se o usuÃ¡rio logado Ã© uma instituiÃ§Ã£o
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(usuarioId, out int id))
                return Unauthorized();

            // Buscar a instituiÃ§Ã£o do usuÃ¡rio logado
            var instituicao = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoId == id);
            
            if (instituicao == null)
                return Unauthorized("Apenas instituiÃ§Ãµes podem gerenciar medicamentos.");

            // Buscar medicamentos da instituiÃ§Ã£o
            var medicamentos = await _context.Medicamentos
                .Include(m => m.Instituicao)
                .Where(m => m.InstituicaoId == instituicao.InstituicaoId)
                .OrderByDescending(m => m.DataCadastro)
                .ToListAsync();

            // Calcular estatÃ­sticas
            var total = medicamentos.Count;
            var escassez = medicamentos.Count(m => m.EmEscassez);
            var baixo = medicamentos.Count(m => !m.EmEscassez && (double)m.QuantidadeAtual / m.QuantidadeNecessaria < 0.5);
            var normal = medicamentos.Count(m => !m.EmEscassez && (double)m.QuantidadeAtual / m.QuantidadeNecessaria >= 0.5);

            ViewBag.TotalMedicamentos = total;
            ViewBag.MedicamentosEscassez = escassez;
            ViewBag.MedicamentosEstoqueBaixo = baixo;
            ViewBag.MedicamentosNormais = normal;

            return View(medicamentos);
        }

        // GET: Medicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)
                return NotFound();

            // Verificar se o medicamento pertence Ã  instituiÃ§Ã£o do usuÃ¡rio logado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioId, out int userId) && medicamento.InstituicaoId != userId)
                return Forbid();

            return View(medicamento);
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Categoria,Dosagem,FormaFarmaceutica,NivelPrioridade,QuantidadeNecessaria,QuantidadeAtual,Observacoes")] Medicamento medicamento)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(usuarioId, out int id))
                return Unauthorized();

            // Verificar se Ã© uma instituiÃ§Ã£o
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null)
                return Unauthorized("Apenas instituiÃ§Ãµes podem cadastrar medicamentos.");

            if (ModelState.IsValid)
            {
                medicamento.InstituicaoId = id;
                medicamento.DataCadastro = DateTime.Now;
                medicamento.UltimaAtualizacao = DateTime.Now;
                medicamento.Ativo = true;

                _context.Add(medicamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Medicamento '{medicamento.Nome}' cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            return View(medicamento);
        }

        // GET: Medicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
                return NotFound();

            // Verificar se o medicamento pertence Ã  instituiÃ§Ã£o do usuÃ¡rio logado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioId, out int userId) && medicamento.InstituicaoId != userId)
                return Forbid();

            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Categoria,Dosagem,FormaFarmaceutica,NivelPrioridade,QuantidadeNecessaria,QuantidadeAtual,Observacoes,Ativo,InstituicaoId,DataCadastro")] Medicamento medicamento)
        {
            if (id != medicamento.Id)
                return NotFound();

            // Verificar se o medicamento pertence Ã  instituiÃ§Ã£o do usuÃ¡rio logado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioId, out int userId) && medicamento.InstituicaoId != userId)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    medicamento.UltimaAtualizacao = DateTime.Now;
                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Medicamento '{medicamento.Nome}' atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicamento);
        }

        // GET: Medicamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos
                .Include(m => m.Instituicao)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicamento == null)
                return NotFound();

            // Verificar se o medicamento pertence Ã  instituiÃ§Ã£o do usuÃ¡rio logado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioId, out int userId) && medicamento.InstituicaoId != userId)
                return Forbid();

            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
>>>>>>> Stashed changes
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
<<<<<<< Updated upstream
            var item = await _context.InstituicaoMedicamentos.FirstOrDefaultAsync(i => i.InstituicaoMedicamentoId == id);
            if (item == null) return NotFound();
            _context.InstituicaoMedicamentos.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
=======
            var medicamento = await _context.Medicamentos.FindAsync(id);
            
            if (medicamento == null)
                return NotFound();

            // Verificar se o medicamento pertence Ã  instituiÃ§Ã£o do usuÃ¡rio logado
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioId, out int userId) && medicamento.InstituicaoId != userId)
                return Forbid();

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Medicamento '{medicamento.Nome}' excluÃ­do com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }

        // API para buscar medicamentos em escassez (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetMedicamentosEscassez()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(usuarioId, out int id))
                return Unauthorized();

            var medicamentosEscassez = await _context.Medicamentos
                .Where(m => m.InstituicaoId == id && m.Ativo)
                .ToListAsync();

            var escassez = medicamentosEscassez
                .Where(m => m.EmEscassez)
                .Select(m => new
                {
                    m.Id,
                    m.Nome,
                    m.Categoria,
                    m.QuantidadeAtual,
                    m.QuantidadeNecessaria,
                    m.NivelPrioridade,
                    Porcentagem = (double)m.QuantidadeAtual / m.QuantidadeNecessaria * 100
                })
                .OrderByDescending(m => m.NivelPrioridade)
                .ThenBy(m => m.Porcentagem)
                .ToList();

            return Json(escassez);
        }
    }
}
>>>>>>> Stashed changes
