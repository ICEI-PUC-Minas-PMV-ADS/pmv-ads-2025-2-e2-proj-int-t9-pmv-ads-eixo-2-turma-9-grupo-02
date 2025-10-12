using MedShare.Context;
using MedShare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class DoacoesCreateModel : PageModel
{
    private readonly AppDbContext _context;
    public DoacoesCreateModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Doacao Doacao { get; set; }

    public void OnGet()
    {
        // Pode carregar dados auxiliares aqui se necessário
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        _context.Doacaos.Add(Doacao);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
