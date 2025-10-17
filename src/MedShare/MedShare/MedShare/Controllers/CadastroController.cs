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
    }
}