using MedShare.Context;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class DoacoesDeleteModel : PageModel
{
    private readonly AppDbContext _context;
    public DoacoesDeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Doacao Doacao { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Doacao = await _context.Doacaos.FindAsync(id);
        if (Doacao == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var doacao = await _context.Doacaos.FindAsync(id);
        if (doacao != null)
        {
            _context.Doacaos.Remove(doacao);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("Index");
    }
}
