// ViewModel para agrupar listas de contas do sistema.

using MedShare.Models;
using System.Collections.Generic;

namespace MedShare.Models
{
    public class ContasViewModel
    {
        public List<Usuario> Usuarios { get; set; }
        public List<Doador> Doadores { get; set; }
        public List<Instituicao> Instituicoes { get; set; }
    }
}
