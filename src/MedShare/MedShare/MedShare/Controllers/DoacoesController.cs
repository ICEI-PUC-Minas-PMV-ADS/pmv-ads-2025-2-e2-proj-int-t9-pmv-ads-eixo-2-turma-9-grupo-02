using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace MedShare.Controllers
{
    public class DoacoesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DoacoesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Doacoes.ToListAsync();
            return View(dados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doacao doacao)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                if (doacao.FotoDoacao != null)
                {
                    string fotoFileName = Guid.NewGuid() + Path.GetExtension(doacao.FotoDoacao.FileName);
                    string fotoPath = Path.Combine(uploadsFolder, fotoFileName);

                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }

                    doacao.CaminhoFoto = "/uploads/" + fotoFileName;
                }

                if (doacao.ReceitaDoacao != null)
                {
                    string receitaFileName = Guid.NewGuid() + Path.GetExtension(doacao.ReceitaDoacao.FileName);
                    string receitaPath = Path.Combine(uploadsFolder, receitaFileName);

                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }

                    doacao.CaminhoReceita = "/uploads/" + receitaFileName;
                }

                _context.Add(doacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(doacao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return NotFound();

            return View(doacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doacao doacao)
        {
            if (id != doacao.Id) return NotFound();
            ModelState.Remove(nameof(doacao.FotoDoacao));
            ModelState.Remove(nameof(doacao.ReceitaDoacao));

            var doacaoExistente = await _context.Doacoes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (doacaoExistente == null) return NotFound();

            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Se um novo arquivo foi enviado, Doacao.FotoDoacao conterá o arquivo via model binding.
                if (doacao.FotoDoacao != null && doacao.FotoDoacao.Length > 0)
                {
                    string fotoFileName = Guid.NewGuid().ToString() + Path.GetExtension(doacao.FotoDoacao.FileName);
                    string fotoPath = Path.Combine(uploadsFolder, fotoFileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoFoto = "/uploads/" + fotoFileName;
                }
                else
                {
                    doacao.CaminhoFoto = doacaoExistente.CaminhoFoto;
                }

                if (doacao.ReceitaDoacao != null && doacao.ReceitaDoacao.Length > 0)
                {
                    string receitaFileName = Guid.NewGuid().ToString() + Path.GetExtension(doacao.ReceitaDoacao.FileName);
                    string receitaPath = Path.Combine(uploadsFolder, receitaFileName);
                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }
                    doacao.CaminhoReceita = "/uploads/" + receitaFileName;
                }
                else
                {
                    doacao.CaminhoReceita = doacaoExistente.CaminhoReceita;
                }

                try
                {
                    _context.Update(doacao);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doacoes.Any(e => e.Id == doacao.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(doacao);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return NotFound();

            return View(doacao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return NotFound();

            return View(doacao);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return NotFound();

            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return NotFound();

            _context.Doacoes.Remove(doacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
