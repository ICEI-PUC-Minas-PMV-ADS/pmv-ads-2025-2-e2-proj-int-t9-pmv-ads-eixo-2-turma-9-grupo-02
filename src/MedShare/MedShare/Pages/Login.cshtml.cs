using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedShare.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Senha { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // TODO: Adicionar l�gica de autentica��o
            return Page();
        }
    }
}
