using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MedShare.Pages
{
    public class CadastroPessoaJuridicaModel : PageModel
    {
        [BindProperty]
        public string RazaoSocial { get; set; }
        [BindProperty]
        public string CNPJ { get; set; }
        [BindProperty]
        public string EmailCorporativo { get; set; }
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
