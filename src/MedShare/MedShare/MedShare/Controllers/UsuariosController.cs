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
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // Rota GET Usuarios
        public async Task<IActionResult> Index()
        {
            var model = new MedShare.Models.ContasViewModel
            {
                Usuarios = await _context.Usuarios.ToListAsync(),
                Doadores = await _context.Doadores.ToListAsync(),
                Instituicoes = await _context.Instituicoes.ToListAsync()
            };
            return View(model);
        }

        [AllowAnonymous]
        //Rota get Login
        public IActionResult Login() 
        {
            // Impede acesso à tela de login se já estiver autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
            return View();
        }

        [AllowAnonymous]
        //Rota post Login
        [HttpPost]
        public async Task<IActionResult> Login(string UsuarioEmail, string UsuarioSenha, string perfil)
        {
            object dados = null;
            bool senhaOk = false;

            if (perfil == "Usuario")
            {
                dados = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioEmail == UsuarioEmail);
                if (dados != null)
                {
                    var usuario = (Usuario)dados;
                    // Verifica se a senha está em formato BCrypt
                    if (usuario.UsuarioSenha != null && usuario.UsuarioSenha.StartsWith("$2a$") || usuario.UsuarioSenha.StartsWith("$2b$") || usuario.UsuarioSenha.StartsWith("$2y$"))
                    {
                        senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, usuario.UsuarioSenha);
                    }
                    else
                    {
                        // Se não estiver, compara diretamente (apenas para testes, não recomendado em produção)
                        senhaOk = usuario.UsuarioSenha == UsuarioSenha;
                    }
                }
            }
            else if (perfil == "Doador")
            {
                dados = await _context.Doadores.FirstOrDefaultAsync(d => d.DoadorEmail == UsuarioEmail);
                if (dados != null)
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, ((Doador)dados).DoadorSenha);
            }
            else if (perfil == "Instituicao")
            {
                dados = await _context.Instituicoes.FirstOrDefaultAsync(i => i.InstituicaoEmail == UsuarioEmail);
                if (dados != null)
                    senhaOk = BCrypt.Net.BCrypt.Verify(UsuarioSenha, ((Instituicao)dados).InstituicaoSenha);
            }

            if (dados == null || !senhaOk)
            {
                ViewBag.Message = "Usuário ou senha inválidos!";
                return View();
            }

            // Claims para autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UsuarioEmail),
                new Claim(ClaimTypes.Role, perfil)
            };

            var usuarioIdentity = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal principal = new ClaimsPrincipal(usuarioIdentity);

            var props = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.ToLocalTime().AddHours(8),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(principal, props);

            return Redirect("/");
        }

        /*Rota logout */
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Usuarios");
        }

        //Rota get para escolher perfil
        [AllowAnonymous]
        public IActionResult EscolherPerfil() {
            return View();
        }


        // Rota get Create de Doador
        [AllowAnonymous]
        public IActionResult CreateDoador() {
            return View();
        }

        // Rota post Create de Doador
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDoador(Doador doador) {
            if (ModelState.IsValid) {
                // Criptografa a senha antes de salvar
                doador.DoadorSenha = BCrypt.Net.BCrypt.HashPassword(doador.DoadorSenha);
                _context.Doadores.Add(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Usuarios");
            }
            return View(doador);
        }

        // Rota get Create de Instituicao
        [AllowAnonymous]
        public IActionResult CreateInstituicao() {
            return View();
        }

        // Rota post Create de Instituicao
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInstituicao(Instituicao instituicao) {
            if (ModelState.IsValid) {
                // Criptografa a senha antes de salvar
                instituicao.InstituicaoSenha = BCrypt.Net.BCrypt.HashPassword(instituicao.InstituicaoSenha);
                _context.Instituicoes.Add(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Usuarios");
            }
            return View(instituicao);
        }



        /*
          [AllowAnonymous]
         // GET: Usuarios/Create
         public IActionResult Create()
         {
             return View();
         }

         // POST: Usuarios/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to.
         // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [AllowAnonymous]
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("UsuarioId,UsuarioEmail,UsuarioSenha,Perfil")] Usuario usuario)
         {
             if (ModelState.IsValid)
             {
                 assim que é passado a senha é feita a criptografia
        usuario.UsuarioSenha = BCrypt.Net.BCrypt.HashPassword(usuario.UsuarioSenha); 
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
             }
            return View(usuario);
         }

        */

        // Removido: Create, Edit, Delete, Details pois agora o cadastro e edição são feitos por CreateDoador/CreateInstituicao e a listagem é feita por Index agrupando todos os tipos.
    }
}
