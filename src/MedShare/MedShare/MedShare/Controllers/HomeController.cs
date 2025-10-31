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
            if (User.IsInRole("Doador"))
                return RedirectToAction("HomePageDoador", "Home");
            else if (User.IsInRole("Instituicao"))
                return RedirectToAction("HomePageInstituicao", "Home");
            else
                return RedirectToAction("Login", "Auth");
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

        [AllowAnonymous]
        public IActionResult HomePageDoador()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (!User.IsInRole("Doador"))
                return RedirectToAction("HomePageInstituicao", "Home");
            return View();
        }

        [AllowAnonymous]
        public IActionResult HomePageInstituicao()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            if (!User.IsInRole("Instituicao"))
                return RedirectToAction("HomePageDoador", "Home");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

// Controller responsável pelas páginas principais e navegação do sistema.
