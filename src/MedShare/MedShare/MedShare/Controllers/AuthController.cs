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
using Microsoft.Extensions.Logging;

namespace MedShare.Controllers
{
    // Controller responsável pela autenticação e login dos usuários.
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
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
            _logger.LogInformation($"Tentativa de login: Email={UsuarioEmail}, Perfil={perfil}");
            object dados = null;
            bool senhaOk = false;

            if (perfil == "Usuario" || perfil == "Admin")
            {
                dados = await _context.Usuarios.FirstOrDefaultAsync(m => m.UsuarioEmail == UsuarioEmail);
                _logger.LogInformation($"Usuario encontrado: {(dados != null ? "Sim" : "Não")}");
                if (dados != null)
                {
                    Usuario usuario = (Usuario)dados;
                    // Para admin, compara senha em texto puro
                    if (usuario.Perfil == Perfil.Admin)
                        senhaOk = UsuarioSenha == usuario.UsuarioSenha;
                    else
                        senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, usuario.UsuarioSenha);
                    _logger.LogInformation($"Senha correta: {senhaOk}");
                    _logger.LogInformation($"Perfil do usuario no banco: {usuario.Perfil}");
                }
            }
            else if (perfil == "Doador")
            {
                dados = await _context.Doadores.FirstOrDefaultAsync(m => m.DoadorEmail == UsuarioEmail);
                _logger.LogInformation($"Doador encontrado: {(dados != null ? "Sim" : "Não")}");
                if (dados != null)
                {
                    Doador doador = (Doador)dados;
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, doador.DoadorSenha);
                    _logger.LogInformation($"Senha correta: {senhaOk}");
                }
            }
            else if (perfil == "Instituicao")
            {
                dados = await _context.Instituicoes.FirstOrDefaultAsync(m => m.InstituicaoEmail == UsuarioEmail);
                _logger.LogInformation($"Instituicao encontrada: {(dados != null ? "Sim" : "Não")}");
                if (dados != null)
                {
                    Instituicao instituicao = (Instituicao)dados;
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, instituicao.InstituicaoSenha);
                    _logger.LogInformation($"Senha correta: {senhaOk}");
                }
            }

            if (dados != null && senhaOk)
            {
                var claims = new List<Claim>();

                if (perfil == "Usuario" || perfil == "Admin")
                {
                    Usuario usuario = (Usuario)dados;
                    claims.Add(new Claim(ClaimTypes.Name, usuario.UsuarioEmail));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()));
                    claims.Add(new Claim("UsuarioEmail", usuario.UsuarioEmail));
                    claims.Add(new Claim(ClaimTypes.Role, usuario.Perfil.ToString()));
                }
                else if (perfil == "Doador")
                {
                    Doador doador = (Doador)dados;
                    claims.Add(new Claim(ClaimTypes.Name, doador.DoadorNome));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, doador.DoadorId.ToString()));
                    claims.Add(new Claim("DoadorEmail", doador.DoadorEmail));
                    claims.Add(new Claim(ClaimTypes.Role, "Doador"));
                    // Atualiza último login
                    doador.UltimoLogin = DateTime.Now;
                    _context.Doadores.Update(doador);
                    await _context.SaveChangesAsync();
                }
                else if (perfil == "Instituicao")
                {
                    Instituicao instituicao = (Instituicao)dados;
                    claims.Add(new Claim(ClaimTypes.Name, instituicao.InstituicaoNome));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, instituicao.InstituicaoId.ToString()));
                    claims.Add(new Claim("InstituicaoEmail", instituicao.InstituicaoEmail));
                    claims.Add(new Claim(ClaimTypes.Role, "Instituicao"));
                    // Atualiza último login
                    instituicao.UltimoLogin = DateTime.Now;
                    _context.Instituicoes.Update(instituicao);
                    await _context.SaveChangesAsync();
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
                else if (perfil == "Admin")
                    return RedirectToAction("HomePageAdmin", "Home");
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