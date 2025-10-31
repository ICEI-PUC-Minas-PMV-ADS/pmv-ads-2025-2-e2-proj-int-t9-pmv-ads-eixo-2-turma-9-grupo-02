using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedShare.Controllers
{
    // Controller responsável pelo cadastro e edição de doadores e instituições.
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Cadastro/CreateDoador
        public IActionResult CreateDoador()
        {
            // Impede acesso à tela de cadastro se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Cadastro/CreateDoador
        public async Task<IActionResult> CreateDoador(Doador doador)
        {
            if (ModelState.IsValid)
            {
                // Criptografa a senha antes de salvar
                doador.DoadorSenha = BCrypt.Net.BCrypt.HashPassword(doador.DoadorSenha);
                _context.Doadores.Add(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }
            return View(doador);
        }

        [AllowAnonymous]
        // GET: Cadastro/CreateInstituicao
        public IActionResult CreateInstituicao()
        {
            // Impede acesso à tela de cadastro se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Cadastro/CreateInstituicao
        public async Task<IActionResult> CreateInstituicao(Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                // Criptografa a senha antes de salvar
                instituicao.InstituicaoSenha = BCrypt.Net.BCrypt.HashPassword(instituicao.InstituicaoSenha);
                _context.Instituicoes.Add(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }
            return View(instituicao);
        }

        [Authorize]
        // GET: Cadastro/EditarDoador
        public async Task<IActionResult> EditarDoador()
        {
            string email = null;
            if (User.IsInRole("Doador"))
            {
                email = User.Claims.FirstOrDefault(c => c.Type == "DoadorEmail")?.Value;
            }
            else if (User.IsInRole("Usuario"))
            {
                email = User.Identity.Name;
            }
            if (string.IsNullOrEmpty(email)) return Unauthorized();
            var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.DoadorEmail == email);
            if (doador == null) return NotFound();
            return View(doador);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Cadastro/EditarDoador
        public async Task<IActionResult> EditarDoador(Doador model)
        {
            string email = null;
            if (User.IsInRole("Doador"))
            {
                email = User.Claims.FirstOrDefault(c => c.Type == "DoadorEmail")?.Value;
            }
            else if (User.IsInRole("Usuario"))
            {
                email = User.Identity.Name;
            }
            if (string.IsNullOrEmpty(email)) return Unauthorized();
            var doador = await _context.Doadores.FirstOrDefaultAsync(d => d.DoadorEmail == email);
            if (doador == null) return NotFound();
            doador.DoadorNome = model.DoadorNome;
            doador.DoadorCPF = model.DoadorCPF;
            doador.DoadorEmail = model.DoadorEmail;
            if (!string.IsNullOrWhiteSpace(model.DoadorSenha))
                doador.DoadorSenha = BCrypt.Net.BCrypt.HashPassword(model.DoadorSenha);
            await _context.SaveChangesAsync();
            // Redireciona para HomePageDoador após editar
            return RedirectToAction("HomePageDoador", "Home");
        }

        [Authorize]
        // GET: Cadastro/EditarInstituicao
        public async Task<IActionResult> EditarInstituicao()
        {
            string email = null;
            if (User.IsInRole("Instituicao"))
            {
                email = User.Claims.FirstOrDefault(c => c.Type == "InstituicaoEmail")?.Value;
            }
            else if (User.IsInRole("Usuario"))
            {
                email = User.Identity.Name;
            }
            if (string.IsNullOrEmpty(email)) return Unauthorized();
            var inst = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoEmail == email);
            if (inst == null) return NotFound();
            return View(inst);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Cadastro/EditarInstituicao
        public async Task<IActionResult> EditarInstituicao(Instituicao model)
        {
            string email = null;
            if (User.IsInRole("Instituicao"))
            {
                email = User.Claims.FirstOrDefault(c => c.Type == "InstituicaoEmail")?.Value;
            }
            else if (User.IsInRole("Usuario"))
            {
                email = User.Identity.Name;
            }
            if (string.IsNullOrEmpty(email)) return Unauthorized();
            var inst = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoEmail == email);
            if (inst == null) return NotFound();
            inst.InstituicaoNome = model.InstituicaoNome;
            inst.InstituicaoCNPJ = model.InstituicaoCNPJ;
            inst.InstituicaoEndereco = model.InstituicaoEndereco;
            inst.InstituicaoEmail = model.InstituicaoEmail;
            if (!string.IsNullOrWhiteSpace(model.InstituicaoSenha))
                inst.InstituicaoSenha = BCrypt.Net.BCrypt.HashPassword(model.InstituicaoSenha);
            await _context.SaveChangesAsync();
            // Redireciona para HomePageInstituicao após editar
            return RedirectToAction("HomePageInstituicao", "Home");
        }
    }
}