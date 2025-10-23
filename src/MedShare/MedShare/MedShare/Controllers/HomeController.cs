using System.Diagnostics;
using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MedShare.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            return View("HomePage");  // Redireciona para a nova HomePage
        }

        [AllowAnonymous]
        public IActionResult HomePage()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Doar(Doacao doacao)
        {
            if (ModelState.IsValid)
            {
                // TODO: Salvar no banco de dados
                // Por enquanto, apenas redireciona para a página inicial
                TempData["Sucesso"] = "Doação cadastrada com sucesso!";
                return RedirectToAction("Index");
            }
            return View(doacao);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
