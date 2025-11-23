using MedShare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedShare.Controllers
{
    [Authorize]
    public class InstituicaoController : Controller
    {
        private readonly AppDbContext _context;

        public InstituicaoController(AppDbContext context)
        {
            _context = context;
        }

        // LISTA 
        public async Task<IActionResult> DoacoesRecebidas()
        {
            var instituicaoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(instituicaoId, out int instId))
                return Unauthorized();

            var dados = await _context.Doacoes
                .Include(d => d.Doador)
                .Where(d => d.InstituicaoId == instId)
                .ToListAsync();

            bool alterou = false;

            foreach (var d in dados)
            {
                if (d.Status == StatusDoacao.Pendente)
                {
                    var expirou = d.DataCriacao.AddHours(48) < DateTime.UtcNow;
                    if (expirou)
                    {
                        d.Status = StatusDoacao.Rejeitado;
                        alterou = true;
                    }
                }
            }

            if (alterou)
                await _context.SaveChangesAsync();

            var doacoesTela = dados
                .Where(d => d.Status != StatusDoacao.Rejeitado &&
                            d.Status != StatusDoacao.Finalizado)
                .ToList();

            return View(doacoesTela);
        }


        [HttpPost]
        public async Task<IActionResult> AlterarStatus(int id, StatusDoacao status)
        {
            var doacao = await _context.Doacoes.FindAsync(id);

            if (doacao == null)
                return NotFound();

            doacao.Status = status;
            _context.Doacoes.Update(doacao);
            await _context.SaveChangesAsync();

            return RedirectToAction("DoacoesRecebidas");
        }

        public async Task<IActionResult> HistoricoDoacoesInstituicao()
        {
            var instituicaoId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(instituicaoId, out int instId))
                return Unauthorized();

            var historico = await _context.Doacoes
                .Include(d => d.Doador)
                .Include(d => d.Instituicao)
                .Where(d => d.InstituicaoId == instId &&
                            (d.Status == StatusDoacao.Rejeitado || d.Status == StatusDoacao.Finalizado))
                .ToListAsync();

            return View(historico);
        }

    }
}
