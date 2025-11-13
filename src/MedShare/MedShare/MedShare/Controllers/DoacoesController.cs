using Microsoft.AspNetCore.Authorization; // Controle de acesso (requer usuário autenticado)
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching; // Para configurar cache (desativado aqui)
using System.IO; // Manipulação de arquivos (uploads de imagens/receitas)
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Operações EF Core (Include, ToListAsync, etc.)
using MedShare.Models; // Modelos do domínio (Doacao, Instituicao, etc.)
using System.Security.Claims; // Recuperar informações do usuário logado (Claims)

namespace MedShare.Controllers
{
    [Authorize] // Exige autenticação para qualquer ação neste controller
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Garante que as respostas não serão cacheadas pelo navegador
    public class DoacoesController : Controller
    {
        private readonly AppDbContext _context; // DbContext injetado para acesso ao banco
        public DoacoesController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAGEM DAS DOAÇÕES DO DOADOR LOGADO
        public async Task<IActionResult> Index()
        {
            // Recupera o ID (NameIdentifier) do usuário logado via Claims
            var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Se não for possível converter para int, usuário não é válido
            if (!int.TryParse(doadorId, out int id))
                return Unauthorized();

            // Busca somente as doações do doador logado incluindo a Instituição associada
            var dados = await _context.Doacoes
                .Include(d => d.Instituicao)
                .Where(d => d.DoadorId == id)
                .ToListAsync();

            return View(dados); // Retorna a lista para a view Index
        }

        // FORMULÁRIO DE CRIAÇÃO DE NOVA DOAÇÃO
        public IActionResult Create()
        {
            // Carrega instituições disponíveis para que o doador selecione uma (fallback)
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(new Doacao()); // Retorna a view com um modelo vazio
        }

        // NOVA AÇÃO (JSON): Retorna instituições que possuem ESCASSEZ CRÍTICA (< 11 caixas) de um medicamento específico
        // Isso permite, no front-end, filtrar dinamicamente as instituições quando o doador digita o nome do medicamento
        [HttpGet]
        public IActionResult InstituicoesPorMedicamento(string nome)
        {
            // Valida entrada vazia
            if (string.IsNullOrWhiteSpace(nome))
                return Json(new { ok = false, itens = new object[0] });

            // Normaliza o termo para comparação case-insensitive
            var termo = nome.Trim().ToLower();

            // Consulta MedicamentosInstituicao onde há escassez (<11 caixas) e o nome coincide
            var itens = _context.MedicamentosInstituicao
                .Include(m => m.Instituicao) // Inclui dados da instituição para montar retorno
                .Where(m => m.QuantidadeCaixas < 11 && m.Nome.ToLower() == termo)
                .Select(m => new
                {
                    id = m.Instituicao.InstituicaoId,
                    nome = m.Instituicao.InstituicaoNome,
                    endereco = m.Instituicao.InstituicaoEndereco
                })
                .Distinct() // Evita duplicidade se houver mais de um registro do mesmo medicamento/instituição
                .ToList();

            // Retorna JSON para consumo via fetch/AJAX na view
            return Json(new { ok = true, itens });
        }

        // RECEBE SUBMISSÃO DA DOAÇÃO (UPLOAD DE FOTO + RECEITA + RELACIONAMENTO COM DOADOR)
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra CSRF
        public async Task<IActionResult> Create(Doacao doacao)
        {
            // Recarrega instituições para repovoar o select em caso de erro de validação
            ViewBag.Instituicoes = await _context.Instituicoes.ToListAsync();

            // Diretório físico onde imagens e receitas serão salvas
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir); // Cria diretório se não existir

            // Recupera valores persistidos em campos hidden (caso já tenham sido enviados anteriormente)
            var form = Request.Form;
            var hiddenCaminhoFoto = form["CaminhoFoto"].FirstOrDefault();
            var hiddenCaminhoReceita = form["CaminhoReceita"].FirstOrDefault();

            // PROCESSA UPLOAD DA FOTO DO MEDICAMENTO
            if (doacao.FotoDoacao != null && doacao.FotoDoacao.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(doacao.FotoDoacao.FileName); // Nome único
                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await doacao.FotoDoacao.CopyToAsync(stream); // Grava arquivo no disco
                }
                doacao.CaminhoFoto = "/images/" + fileName; // Caminho relativo salvo no banco
            }
            else if (string.IsNullOrEmpty(doacao.CaminhoFoto) && !string.IsNullOrEmpty(hiddenCaminhoFoto))
            {
                // Reutiliza caminho já existente (ex: postback)
                doacao.CaminhoFoto = hiddenCaminhoFoto;
            }

            // PROCESSA UPLOAD DA RECEITA DO MEDICAMENTO
            if (doacao.ReceitaDoacao != null && doacao.ReceitaDoacao.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(doacao.ReceitaDoacao.FileName);
                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await doacao.ReceitaDoacao.CopyToAsync(stream);
                }
                doacao.CaminhoReceita = "/images/" + fileName;
            }
            else if (string.IsNullOrEmpty(doacao.CaminhoReceita) && !string.IsNullOrEmpty(hiddenCaminhoReceita))
            {
                doacao.CaminhoReceita = hiddenCaminhoReceita;
            }

            // Remove validação de propriedades que não devem ser validadas diretamente (IFormFile / caminhos)
            ModelState.Remove(nameof(doacao.FotoDoacao));
            ModelState.Remove(nameof(doacao.ReceitaDoacao));
            ModelState.Remove(nameof(doacao.CaminhoFoto));
            ModelState.Remove(nameof(doacao.CaminhoReceita));

            // Valida obrigatoriedade dos arquivos (regras de negócio)
            if (string.IsNullOrEmpty(doacao.CaminhoFoto))
                ModelState.AddModelError("FotoDoacao", "Obrigatório enviar a foto da caixa do medicamento!");

            if (string.IsNullOrEmpty(doacao.CaminhoReceita))
                ModelState.AddModelError("ReceitaDoacao", "Obrigatório enviar a receita do medicamento!");

            // Se tudo válido, associa doador e salva
            if (ModelState.IsValid)
            {
                var doadorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(doadorId, out int id))
                    doacao.DoadorId = id; // Relaciona doação com doador logado

                _context.Add(doacao);
                await _context.SaveChangesAsync(); // Persiste no banco
                return RedirectToAction("Index"); // Volta para lista
            }

            // Retorna view com erros exibidos
            return View(doacao);
        }

        // FORMULÁRIO DE EDIÇÃO DE DOAÇÃO EXISTENTE
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Carrega doação incluindo instituição para edição
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();

            ViewBag.Instituicoes = _context.Instituicoes.ToList(); // Repopula select
            return View(dados);
        }

        // PROCESSA ALTERAÇÕES DA DOAÇÃO
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Doacao doacao)
        {
            if (id != doacao.Id)
                return NotFound(); // ID divergente

            var doacaoExistente = await _context.Doacoes.FindAsync(id);
            if (doacaoExistente == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Atualiza campos editáveis
                doacaoExistente.NomeDoacao = doacao.NomeDoacao;
                doacaoExistente.ValidadeDoacao = doacao.ValidadeDoacao;
                doacaoExistente.QuantidadeDoacao = doacao.QuantidadeDoacao;
                doacaoExistente.InstituicaoId = doacao.InstituicaoId;

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Se nova foto enviada, substitui caminho
                if (doacao.FotoDoacao != null)
                {
                    var fotoPath = Path.Combine(uploadDir, doacao.FotoDoacao.FileName);
                    using (var stream = new FileStream(fotoPath, FileMode.Create))
                    {
                        await doacao.FotoDoacao.CopyToAsync(stream);
                    }
                    doacaoExistente.CaminhoFoto = "/images/" + doacao.FotoDoacao.FileName;
                }

                // Se nova receita enviada, substitui caminho
                if (doacao.ReceitaDoacao != null)
                {
                    var receitaPath = Path.Combine(uploadDir, doacao.ReceitaDoacao.FileName);
                    using (var stream = new FileStream(receitaPath, FileMode.Create))
                    {
                        await doacao.ReceitaDoacao.CopyToAsync(stream);
                    }
                    doacaoExistente.CaminhoReceita = "/images/" + doacao.ReceitaDoacao.FileName;
                }

                _context.Update(doacaoExistente); // Marca entidade como modificada
                await _context.SaveChangesAsync(); // Persiste alterações
                return RedirectToAction("Index");
            }

            // Se inválido, repopula lista de instituições e retorna view
            ViewBag.Instituicoes = _context.Instituicoes.ToList();
            return View(doacao);
        }

        // DETALHES DE UMA DOAÇÃO
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();
            return View(dados);
        }

        // CONFIRMAÇÃO DE EXCLUSÃO
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var dados = await _context.Doacoes.Include(d => d.Instituicao).FirstOrDefaultAsync(d => d.Id == id);
            if (dados == null)
                return NotFound();
            return View(dados);
        }

        // EXECUTA EXCLUSÃO DEFINITIVA
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Doacoes.FindAsync(id);
            if (dados == null)
                return NotFound();

            _context.Doacoes.Remove(dados); // Remove entidade do DbSet
            await _context.SaveChangesAsync(); // Persiste exclusão
            return RedirectToAction("Index");
        }
    }
}