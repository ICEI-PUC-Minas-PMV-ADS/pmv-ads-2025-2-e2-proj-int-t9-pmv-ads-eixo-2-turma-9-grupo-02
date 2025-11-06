using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using System.Security.Claims;
using System.Linq; // Para LINQ (consultas e ordenações)

namespace MedShare.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class DoacoesController : Controller
    {
        private readonly AppDbContext _context;
        public DoacoesController(AppDbContext context)
        {
            _context = context;
        }

        // Lista somente as doações do doador autenticado (filtrando pelo Claim NameIdentifier)
        public async Task<IActionResult> Index()
        {
            var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(doadorId, out int id)) return Unauthorized();
            var dados = await _context.Doacoes
                .Include(d => d.Instituicao) // Inclui instituição para exibir nome/endereço na listagem
                .Where(d => d.DoadorId == id) // Filtra pelo doador
                .ToListAsync();
            return View(dados);
        }

        // GET: Tela de criação de doação
        public IActionResult Create()
        {
            // Lista completa de instituições (será refinada pelo usuário via medicamento necessário)
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            // Catálogo global de medicamentos para sugestão/autocomplete (utilizado na filtragem por escassez)
            ViewBag.Medicamentos = _context.Medicamentos.OrderBy(m => m.Nome).Select(m => m.Nome).ToList();
            return View(new Doacao());
        }

        // GET AJAX: retorna instituições que possuem escassez (< 11 caixas) do medicamento informado.
        // Permite ao doador direcionar a doação para onde há maior necessidade.
        [HttpGet]
        public async Task<IActionResult> InstituicoesNecessitam(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return Json(new { sucesso = false, instituicoes = new object[0] });

            var instituicoes = await _context.InstituicaoMedicamentos
                .Include(im => im.Instituicao)   // Dados da instituição
                .Include(im => im.Medicamento)  // Dados do medicamento
                .Where(im => im.Medicamento.Nome.ToLower() == nome.ToLower() && im.QuantidadeCaixas < 11) // Regra de escassez
                .Select(im => new {
                    id = im.Instituicao.InstituicaoId,
                    nome = im.Instituicao.InstituicaoNome,
                    endereco = im.Instituicao.InstituicaoEndereco,
                    quantidadeAtual = im.QuantidadeCaixas // Estoque atual (baixo)
                })
                .ToListAsync();

            return Json(new { sucesso = true, instituicoes });
        }

        // POST: Criação da doação (faz upload de arquivos e persiste registro)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doacao doacao)
        {
            // Valida presença dos arquivos exigidos
            if (doacao.FotoDoacao == null)
                ModelState.AddModelError("FotoDoacao", "Obrigatório enviar a foto da caixa do medicamento!");
            if (doacao.ReceitaDoacao == null)
                ModelState.AddModelError("ReceitaDoacao", "Obrigatório enviar a receita do medicamento!");

            if (ModelState.IsValid)
            {
                // Diretório centralizado para imagens/documentos (criado se não existir)
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                // Upload da foto do medicamento
                if (doacao.FotoDoacao != null)
                {
                    var fotoPath = Path.Combine(uploadDir, doacao.FotoDoacao.FileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoFoto = "/images/" + doacao.FotoDoacao.FileName; // Caminho salvo no banco
                }
                // Upload da receita (imagem ou PDF)
                if (doacao.ReceitaDoacao != null)
                {
                    var receitaPath = Path.Combine(uploadDir, doacao.ReceitaDoacao.FileName);
                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoReceita = "/images/" + doacao.ReceitaDoacao.FileName;
                }

                // Relaciona doação ao doador autenticado
                var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(doadorId, out int id)) doacao.DoadorId = id;

                _context.Add(doacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Recarrega listas em caso de falha de validação
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            ViewBag.Medicamentos = _context.Medicamentos.OrderBy(m => m.Nome).Select(m => m.Nome).ToList();
            return View(doacao);
        }

        // GET: Edição de doação existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null) return NotFound();
            ViewBag.Instituicoes = _context.Instituicoes.ToList(); // Permite mudança de instituição
            return View(dados);
        }

        // POST: Persistir alterações da doação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doacao doacao)
        {
            if (id != doacao.Id) return NotFound();
            var doacaoExistente = await _context.Doacoes.FindAsync(id);
            if (doacaoExistente == null) return NotFound();

            if (ModelState.IsValid)
            {
                // Atualiza campos básicos
                doacaoExistente.NomeDoacao = doacao.NomeDoacao;
                doacaoExistente.ValidadeDoacao = doacao.ValidadeDoacao;
                doacaoExistente.QuantidadeDoacao = doacao.QuantidadeDoacao;
                doacaoExistente.InstituicaoId = doacao.InstituicaoId;

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                // Se usuário enviou nova foto substitui caminho
                if (doacao.FotoDoacao != null)
                {
                    var fotoPath = Path.Combine(uploadDir, doacao.FotoDoacao.FileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacaoExistente.CaminhoFoto = "/images/" + doacao.FotoDoacao.FileName;
                }
                // Se usuário enviou nova receita substitui caminho
                if (doacao.ReceitaDoacao != null)
                {
                    var receitaPath = Path.Combine(uploadDir, doacao.ReceitaDoacao.FileName);
                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }
                    doacaoExistente.CaminhoReceita = "/images/" + doacao.ReceitaDoacao.FileName;
                }

                _context.Update(doacaoExistente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(doacao);
        }

        // GET: Detalhes da doação
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null) return NotFound();
            return View(dados);
        }

        // GET: Confirmação de exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null) return NotFound();
            return View(dados);
        }

        // POST: Excluir doação
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return NotFound();
            var dados = await _context.Doacoes.FindAsync(id);
            if (dados == null) return NotFound();
            _context.Doacoes.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}