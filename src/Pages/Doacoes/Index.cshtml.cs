using MedShare.Context;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DoacoesIndexModel : PageModel
{
    private readonly AppDbContext _context;
    public DoacoesIndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Doacao> Doacoes { get; set; }

    public async Task OnGetAsync()
    {
        Doacoes = await _context.Doacaos
            .Include(d => d.Doador)
            .Include(d => d.Instituicao)
            .ToListAsync();
    }
}
