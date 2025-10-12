using MedShare.Context;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class DoacoesEditModel : PageModel
{
    private readonly AppDbContext _context;
    public DoacoesEditModel(AppDbContext context)
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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        _context.Attach(Doacao).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
