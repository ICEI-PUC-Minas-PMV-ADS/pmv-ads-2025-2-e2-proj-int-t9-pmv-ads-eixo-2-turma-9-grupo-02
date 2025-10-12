using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using MedShare.Context;

namespace MedShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Busca doações próximas do vencimento (menos de 30 dias)
            var hoje = DateTime.Today;
            var limite = hoje.AddDays(30);
            var doacoesProximas = _context.Doacaos
                .Where(d => d.DoacaoDataValidade <= limite && d.DoacaoDataValidade > hoje)
                .ToList();

            var notificacoes = new List<Notificacao>();
            foreach (var doacao in doacoesProximas)
            {
                var mensagem = $"O medicamento '{doacao.DoacaoNomeMedicamento}' está próximo da validade ({doacao.DoacaoDataValidade:dd/MM/yyyy}).";
                notificacoes.Add(new Notificacao
                {
                    Mensagem = mensagem,
                    DoacaoId = doacao.DoacaoId,
                    DataCriacao = DateTime.Now
                });
            }

            ViewBag.NotificacoesVencimento = notificacoes;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
