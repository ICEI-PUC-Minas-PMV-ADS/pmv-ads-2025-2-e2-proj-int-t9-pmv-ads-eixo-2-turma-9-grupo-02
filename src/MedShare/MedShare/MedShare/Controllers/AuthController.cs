using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedShare.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MedShare.Controllers
{
    // Controller responsável pela autenticação e login dos usuários.
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // Rota GET: Auth/ChooseType
        public IActionResult ChooseType()
        {
            // Impede acesso à tela de escolha de tipo se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            return View();
        }

        [AllowAnonymous]
        // Rota GET: Auth/ChooseTypeRegister
        public IActionResult ChooseTypeRegister()
        {
            // Impede acesso à tela de cadastro de tipo se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            return View();
        }

        [AllowAnonymous]
        // Rota GET: Auth/Login
        public IActionResult Login(string type = null) 
        {
            // Impede acesso à tela de login se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            ViewBag.UserType = type;
            return View();
        }

        //Rota post 
        [AllowAnonymous]
        [HttpPost]
        // POST: Auth/Login
        public async Task<IActionResult> Login(string UsuarioEmail, string UsuarioSenha, string perfil)
        {
            object dados = null;
            bool senhaOk = false;

            if (perfil == "Usuario")
            {
                dados = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioEmail == UsuarioEmail);
                if (dados != null)
                {
                    Usuario usuario = (Usuario)dados;
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, usuario.UsuarioSenha);
                }
            }
            else if (perfil == "Doador")
            {
                dados = await _context.Doadores.FirstOrDefaultAsync(m => m.DoadorEmail == UsuarioEmail);
                if (dados != null)
                {
                    Doador doador = (Doador)dados;
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, doador.DoadorSenha);
                }
            }
            else if (perfil == "Instituicao")
            {
                dados = await _context.Instituicoes.FirstOrDefaultAsync(m => m.InstituicaoEmail == UsuarioEmail);
                if (dados != null)
                {
                    Instituicao instituicao = (Instituicao)dados;
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, instituicao.InstituicaoSenha);
                }
            }

            if (dados != null && senhaOk)
            {
                var claims = new List<Claim>();

                if (perfil == "Usuario")
                {
                    Usuario usuario = (Usuario)dados;
                    claims.Add(new Claim(ClaimTypes.Name, usuario.UsuarioEmail));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()));
                    claims.Add(new Claim("UsuarioEmail", usuario.UsuarioEmail));
                    claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
                }
                else if (perfil == "Doador")
                {
                    Doador doador = (Doador)dados;
                    claims.Add(new Claim(ClaimTypes.Name, doador.DoadorNome));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, doador.DoadorId.ToString()));
                    claims.Add(new Claim("DoadorEmail", doador.DoadorEmail));
                    claims.Add(new Claim(ClaimTypes.Role, "Doador"));
                }
                else if (perfil == "Instituicao")
                {
                    Instituicao instituicao = (Instituicao)dados;
                    claims.Add(new Claim(ClaimTypes.Name, instituicao.InstituicaoNome));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, instituicao.InstituicaoId.ToString()));
                    claims.Add(new Claim("InstituicaoEmail", instituicao.InstituicaoEmail));
                    claims.Add(new Claim(ClaimTypes.Role, "Instituicao"));
                }

                var usuarioIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddHours(8),
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(principal, props);

                if (perfil == "Doador")
                    return RedirectToAction("HomePageDoador", "Home");
                else if (perfil == "Instituicao")
                    return RedirectToAction("HomePageInstituicao", "Home");
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["erro"] = "Email ou senha inválidos!";
                return View();
            }
        }

        [Authorize]
        // Rota GET: Auth/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
/*Anotações [AllowAnonymous] não permitem*/