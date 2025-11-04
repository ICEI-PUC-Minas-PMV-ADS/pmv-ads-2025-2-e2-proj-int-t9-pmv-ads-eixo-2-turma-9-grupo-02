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
    public class DoacoesController : Controller
    {
        private readonly AppDbContext _context;
        public DoacoesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Pega o ID do doador logado
            var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(doadorId, out int id))
                return Unauthorized();

            // Filtra as doações apenas do doador logado
            var dados = await _context.Doacoes
                .Include(d => d.Instituicao)
                .Where(d => d.DoadorId == id)
                .ToListAsync();

            return View(dados);
        }

        public IActionResult Create()
        {
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(new Doacao()); // Garante que o Model não será nulo na view
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doacao doacao)
        {
            // Validação manual dos arquivos
            if (doacao.FotoDoacao == null)
                ModelState.AddModelError("FotoDoacao", "Obrigatório enviar a foto da caixa do medicamento!");
            if (doacao.ReceitaDoacao == null)
                ModelState.AddModelError("ReceitaDoacao", "Obrigatório enviar a receita do medicamento!");

            if (ModelState.IsValid)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                if (doacao.FotoDoacao != null)
                {
                    var fotoPath = Path.Combine(uploadDir, doacao.FotoDoacao.FileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoFoto = "/images/" + doacao.FotoDoacao.FileName;
                }

                if (doacao.ReceitaDoacao != null)
                {
                    var receitaPath = Path.Combine(uploadDir, doacao.ReceitaDoacao.FileName);
                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoReceita = "/images/" + doacao.ReceitaDoacao.FileName;
                }

                var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(doadorId, out int id))
                {
                    doacao.DoadorId = id;
                }

                _context.Add(doacao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(doacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(dados);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Doacao doacao)
        {
            if (id != doacao.Id)
                return NotFound();
            var doacaoExistente = await _context.Doacoes.FindAsync(id);
            if (doacaoExistente == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                doacaoExistente.NomeDoacao = doacao.NomeDoacao;
                doacaoExistente.ValidadeDoacao = doacao.ValidadeDoacao;
                doacaoExistente.QuantidadeDoacao = doacao.QuantidadeDoacao;
                doacaoExistente.InstituicaoId = doacao.InstituicaoId;

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                if (doacao.FotoDoacao != null)
                {
                    var fotoPath = Path.Combine(uploadDir, doacao.FotoDoacao.FileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacaoExistente.CaminhoFoto = "/images/" + doacao.FotoDoacao.FileName;
                }

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
                return RedirectToAction("Index");
            }
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(doacao);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();
            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();
            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Doacoes.FindAsync(id);
            if (dados == null)
                return NotFound();

            _context.Doacoes.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}