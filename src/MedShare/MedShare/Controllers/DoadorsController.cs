using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedShare.Controllers {
    public class DoadorsController : Controller 
    {
        private readonly AppDbContext _context;
        public DoadorsController(AppDbContext context) 
        {
            _context = context;
        }

        /* Rota Index*/
        public async Task<IActionResult> Index() 
        {
            var dados = await _context.Doadors.ToListAsync();
            return View(dados);
        }


        /*Rota Create get */
        public IActionResult Create() 
        {
            return View();
        }

        /*Rota Crate post*/
        [HttpPost]
        public async Task<IActionResult> Create(Doador doador) 
        {
            if (ModelState.IsValid) {
                _context.Doadors.Add(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(doador);
        }

        /*Rota edit get */
        public async Task<IActionResult> Edit(int? id) {

            if (id == null) 
            {
                return NotFound();
            }
            var dados = await _context.Doadors.FindAsync(id);
            if (dados == null) 
            { 
                return NotFound();
            }
            return View(dados);
        }

        /*Rota Edit post*/
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Doador doador) {
            
            if (id != doador.DoadorId) {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                _context.Doadors.Update(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View();
        }

        /*Rota details get */
        public async Task<IActionResult> Details(int? id) 
        {
            if(id == null) 
            {
                return NotFound();
            }
            var dados = await _context.Doadors.FindAsync(id);
            if(dados == null) 
            {
                return NotFound();
            }
            return View(dados);
        }

        /*Rota delete get*/
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null) {
                return NotFound();
            }
            var dados = await _context.Doadors.FindAsync(id);
            if (dados == null) {
                return NotFound();
            }
            return View(dados);
        }

        /*Rota delete post */
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id) {
            if (id == null) {
                return NotFound();
            }
            var dados = await _context.Doadors.FindAsync(id);
            if (dados == null) {
                return NotFound();
            }
            _context.Doadors.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
