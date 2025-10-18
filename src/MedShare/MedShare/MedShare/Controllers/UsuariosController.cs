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
        [Authorize(Roles = "Usuario")]
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

        // Rota get edit Doador
        [Authorize]
        public IActionResult EditDoador()
        {
            var email = User.Identity.Name;
            var doador = _context.Doadores.FirstOrDefault(d => d.DoadorEmail == email);
            if (doador == null) return RedirectToAction("Login", "Usuarios");
            return View(doador);
        }

        //Rora post edit Doador
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoador(Doador doador)
        {
            if (ModelState.IsValid)
            {
                doador.DoadorSenha = BCrypt.Net.BCrypt.HashPassword(doador.DoadorSenha);
                _context.Doadores.Update(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(doador);
        }

        //Rota get edit Instituição
        [Authorize]
        public IActionResult EditInstituicao()
        {
            var email = User.Identity.Name;
            var instituicao = _context.Instituicoes.FirstOrDefault(i => i.InstituicaoEmail == email);
            if (instituicao == null) return RedirectToAction("Login", "Usuarios");
            return View(instituicao);
        }

        //Rota post edit Instituição
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstituicao(Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                instituicao.InstituicaoSenha = BCrypt.Net.BCrypt.HashPassword(instituicao.InstituicaoSenha);
                _context.Instituicoes.Update(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(instituicao);
        }

        //Rota get EditUsuario
        [Authorize]
        public IActionResult EditUsuario()
        {
            var email = User.Identity.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioEmail == email);
            if (usuario == null) return RedirectToAction("Login", "Usuarios");
            return View(usuario);
        }

        //Rota post EditUsuario
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.UsuarioSenha = BCrypt.Net.BCrypt.HashPassword(usuario.UsuarioSenha);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        // Rota get EditUsuario por ID
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> EditUsuarioById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // Rota post EditUsuario por ID
        [HttpPost]
        [Authorize(Roles = "Usuario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUsuarioById(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.UsuarioSenha = BCrypt.Net.BCrypt.HashPassword(usuario.UsuarioSenha);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // Rota get EditDoador por ID
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> EditDoadorById(int id)
        {
            var doador = await _context.Doadores.FindAsync(id);
            if (doador == null) return NotFound();
            return View(doador);
        }

        // Rota post EditDoador por ID
        [HttpPost]
        [Authorize(Roles = "Usuario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoadorById(Doador doador)
        {
            if (ModelState.IsValid)
            {
                doador.DoadorSenha = BCrypt.Net.BCrypt.HashPassword(doador.DoadorSenha);
                _context.Doadores.Update(doador);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(doador);
        }

        // Rota get EditInstituicao por ID
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> EditInstituicaoById(int id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null) return NotFound();
            return View(instituicao);
        }

        // Rota post EditInstituicao por ID
        [HttpPost]
        [Authorize(Roles = "Usuario")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstituicaoById(Instituicao instituicao)
        {
            if (ModelState.IsValid)
            {
                instituicao.InstituicaoSenha = BCrypt.Net.BCrypt.HashPassword(instituicao.InstituicaoSenha);
                _context.Instituicoes.Update(instituicao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instituicao);
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteDoador(int id)
        {
            var doador = await _context.Doadores.FindAsync(id);
            if (doador == null) return NotFound();
            _context.Doadores.Remove(doador);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteInstituicao(int id)
        {
            var instituicao = await _context.Instituicoes.FindAsync(id);
            if (instituicao == null) return NotFound();
            _context.Instituicoes.Remove(instituicao);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteMultipleUsuarios(int[] usuarioIds)
        {
            if (usuarioIds != null && usuarioIds.Length > 0)
            {
                var usuarios = _context.Usuarios.Where(u => usuarioIds.Contains(u.UsuarioId));
                _context.Usuarios.RemoveRange(usuarios);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteMultipleDoadores(int[] doadorIds)
        {
            if (doadorIds != null && doadorIds.Length > 0)
            {
                var doadores = _context.Doadores.Where(d => doadorIds.Contains(d.DoadorId));
                _context.Doadores.RemoveRange(doadores);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Usuario")]
        public async Task<IActionResult> DeleteMultipleInstituicoes(int[] instituicaoIds)
        {
            if (instituicaoIds != null && instituicaoIds.Length > 0)
            {
                var instituicoes = _context.Instituicoes.Where(i => instituicaoIds.Contains(i.InstituicaoId));
                _context.Instituicoes.RemoveRange(instituicoes);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
