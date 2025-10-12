using MedShare.Context;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedShare.Controllers {
    public class DoadorsController : Controller {
        private readonly AppDbContext _context;
        public DoadorsController(AppDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var dados = await _context.Doadors.ToListAsync();

            return View(dados);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doador doador) 
        {
            if (ModelState.IsValid) 
            {
                _context.Doadors.Add(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(doador);
        }
    }
}
