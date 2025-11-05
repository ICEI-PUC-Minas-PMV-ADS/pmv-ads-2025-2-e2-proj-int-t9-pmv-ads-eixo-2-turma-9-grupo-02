using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedShare.Pages
{
    public class CadastroPessoaFisicaModel : PageModel
    {
        [BindProperty]
        public string NomeCompleto { get; set; }
        [BindProperty]
        public string CPF { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string CEP { get; set; }
        [BindProperty]
        public string Endereco { get; set; }
        [BindProperty]
        public bool AceitarTermos { get; set; }

        public void OnGet() { }
        public IActionResult OnPost()
        {
            // TODO: Salvar dados no banco
            return RedirectToPage("/Login");
        }
    }
}
