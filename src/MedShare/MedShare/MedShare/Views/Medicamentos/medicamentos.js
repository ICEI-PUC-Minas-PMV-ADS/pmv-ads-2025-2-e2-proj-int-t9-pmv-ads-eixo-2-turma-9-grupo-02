// ================================================
// JAVASCRIPT PARA GESTÃO DE MEDICAMENTOS
// ================================================

document.addEventListener('DOMContentLoaded', function() {
    // Inicializar tooltips do Bootstrap
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Configurar event listeners para filtros
    document.getElementById('filtroNome').addEventListener('input', filtrarMedicamentos);
    document.getElementById('filtroCategoria').addEventListener('change', filtrarMedicamentos);
    document.getElementById('filtroStatus').addEventListener('change', filtrarMedicamentos);
    document.getElementById('filtroPrioridade').addEventListener('change', filtrarMedicamentos);

    // Atualizar contadores iniciais
    atualizarContadores();
});

// Função para filtrar medicamentos
function filtrarMedicamentos() {
    const nome = document.getElementById('filtroNome').value.toLowerCase();
    const categoria = document.getElementById('filtroCategoria').value;
    const status = document.getElementById('filtroStatus').value;
    const prioridade = document.getElementById('filtroPrioridade').value;
    
    const linhas = document.querySelectorAll('#tabelaMedicamentos tbody tr');
    let visiveisCount = 0;
    let escassezCount = 0;
    let baixoCount = 0;
    let normalCount = 0;
    
    linhas.forEach(linha => {
        const nomeMatch = linha.dataset.nome.includes(nome);
        const categoriaMatch = !categoria || linha.dataset.categoria === categoria;
        const statusMatch = !status || linha.dataset.status === status;
        const prioridadeMatch = !prioridade || linha.dataset.prioridade === prioridade;
        
        const mostrar = nomeMatch && categoriaMatch && statusMatch && prioridadeMatch;
        
        if (mostrar) {
            linha.style.display = '';
            visiveisCount++;
            
            // Contar por status para atualizar cards
            const statusLinha = linha.dataset.status;
            if (statusLinha === 'escassez') escassezCount++;
            else if (statusLinha === 'baixo') baixoCount++;
            else if (statusLinha === 'normal') normalCount++;
        } else {
            linha.style.display = 'none';
        }
    });
    
    // Atualizar cards com contadores filtrados
    document.getElementById('totalMedicamentos').textContent = visiveisCount;
    document.getElementById('medicamentosEscassez').textContent = escassezCount;
    document.getElementById('medicamentosEstoqueBaixo').textContent = baixoCount;
    document.getElementById('medicamentosNormais').textContent = normalCount;
    
    // Mostrar/ocultar alerta de escassez
    const alertasEscassez = document.getElementById('alertasEscassez');
    const numeroEscassez = document.getElementById('numeroEscassez');
    
    if (escassezCount > 0) {
        alertasEscassez.style.display = '';
        numeroEscassez.textContent = escassezCount;
    } else {
        alertasEscassez.style.display = 'none';
    }
    
    // Mostrar mensagem se nenhum resultado
    mostrarMensagemVazia(visiveisCount === 0);
}

// Função para limpar filtros
function limparFiltros() {
    document.getElementById('filtroNome').value = '';
    document.getElementById('filtroCategoria').value = '';
    document.getElementById('filtroStatus').value = '';
    document.getElementById('filtroPrioridade').value = '';
    filtrarMedicamentos();
}

// Função para filtrar apenas medicamentos em escassez
function filtrarEscassez() {
    document.getElementById('filtroStatus').value = 'escassez';
    filtrarMedicamentos();
    
    // Scroll suave para a tabela
    document.getElementById('tabelaMedicamentos').scrollIntoView({ 
        behavior: 'smooth',
        block: 'start' 
    });
}

// Função para exportar relatório (placeholder)
function exportarRelatorio() {
    // Coletar dados visíveis da tabela
    const linhasVisiveis = Array.from(document.querySelectorAll('#tabelaMedicamentos tbody tr'))
        .filter(linha => linha.style.display !== 'none');
    
    if (linhasVisiveis.length === 0) {
        alert('Nenhum medicamento para exportar. Ajuste os filtros e tente novamente.');
        return;
    }
    
    // Simular exportação
    const loading = document.createElement('div');
    loading.className = 'alert alert-info';
    loading.innerHTML = `
        <i class="fas fa-spinner fa-spin me-2"></i>
        Preparando relatório com ${linhasVisiveis.length} medicamento(s)...
    `;
    
    document.querySelector('.container-fluid').insertBefore(loading, document.querySelector('.row'));
    
    setTimeout(() => {
        loading.remove();
        alert(`Relatório gerado com ${linhasVisiveis.length} medicamento(s)!`);
        // Aqui seria implementada a exportação real (PDF, Excel, etc.)
    }, 2000);
}

// Função para atualizar contadores iniciais
function atualizarContadores() {
    const linhas = document.querySelectorAll('#tabelaMedicamentos tbody tr');
    let total = linhas.length;
    let escassez = 0;
    let baixo = 0;
    let normal = 0;
    
    linhas.forEach(linha => {
        const status = linha.dataset.status;
        if (status === 'escassez') escassez++;
        else if (status === 'baixo') baixo++;
        else if (status === 'normal') normal++;
    });
    
    document.getElementById('totalMedicamentos').textContent = total;
    document.getElementById('medicamentosEscassez').textContent = escassez;
    document.getElementById('medicamentosEstoqueBaixo').textContent = baixo;
    document.getElementById('medicamentosNormais').textContent = normal;
    
    // Mostrar alerta se houver medicamentos em escassez
    const alertasEscassez = document.getElementById('alertasEscassez');
    const numeroEscassez = document.getElementById('numeroEscassez');
    
    if (escassez > 0) {
        alertasEscassez.style.display = '';
        numeroEscassez.textContent = escassez;
    } else {
        alertasEscassez.style.display = 'none';
    }
}

// Função para mostrar mensagem quando não há resultados
function mostrarMensagemVazia(mostrar) {
    let mensagemExistente = document.getElementById('mensagemVazia');
    
    if (mostrar && !mensagemExistente) {
        const tbody = document.querySelector('#tabelaMedicamentos tbody');
        const mensagem = document.createElement('tr');
        mensagem.id = 'mensagemVazia';
        mensagem.innerHTML = `
            <td colspan="9" class="text-center py-5">
                <i class="fas fa-search fa-3x text-muted mb-3"></i>
                <h5 class="text-muted">Nenhum medicamento encontrado</h5>
                <p class="text-muted">Tente ajustar os filtros de busca</p>
                <button class="btn btn-outline-primary" onclick="limparFiltros()">
                    <i class="fas fa-eraser me-1"></i>Limpar Filtros
                </button>
            </td>
        `;
        tbody.appendChild(mensagem);
    } else if (!mostrar && mensagemExistente) {
        mensagemExistente.remove();
    }
}

// Função para confirmar exclusão
function confirmarExclusao(nomeMedicamento) {
    return confirm(`Tem certeza que deseja excluir o medicamento "${nomeMedicamento}"?\n\nEsta ação não pode ser desfeita.`);
}

// Função para mostrar detalhes rápidos (tooltip avançado)
function mostrarDetalhesRapidos(elemento, dados) {
    // Implementar tooltip customizado com mais informações
    // Pode ser usado para mostrar informações adicionais ao passar o mouse
}

// Função para alternar tema (se necessário)
function alternarTema() {
    document.body.classList.toggle('dark-theme');
    localStorage.setItem('tema', document.body.classList.contains('dark-theme') ? 'dark' : 'light');
}

// Função para imprimir tabela
function imprimirTabela() {
    const conteudoOriginal = document.body.innerHTML;
    const tabela = document.getElementById('tabelaMedicamentos').outerHTML;
    
    document.body.innerHTML = `
        <div class="container mt-4">
            <h2>Relatório de Medicamentos - MedShare</h2>
            <p>Data: ${new Date().toLocaleDateString('pt-BR')}</p>
            ${tabela}
        </div>
    `;
    
    window.print();
    document.body.innerHTML = conteudoOriginal;
    
    // Reativar event listeners após restaurar conteúdo
    document.dispatchEvent(new Event('DOMContentLoaded'));
}

// Função para busca avançada (pode ser expandida)
function buscarAvancada() {
    // Modal ou página separada para busca com mais opções
    alert('Funcionalidade de busca avançada será implementada em breve!');
}

// Função para salvar filtros no localStorage
function salvarFiltros() {
    const filtros = {
        nome: document.getElementById('filtroNome').value,
        categoria: document.getElementById('filtroCategoria').value,
        status: document.getElementById('filtroStatus').value,
        prioridade: document.getElementById('filtroPrioridade').value
    };
    localStorage.setItem('filtrosMedicamentos', JSON.stringify(filtros));
}

// Função para carregar filtros salvos
function carregarFiltros() {
    const filtrosSalvos = localStorage.getItem('filtrosMedicamentos');
    if (filtrosSalvos) {
        const filtros = JSON.parse(filtrosSalvos);
        document.getElementById('filtroNome').value = filtros.nome || '';
        document.getElementById('filtroCategoria').value = filtros.categoria || '';
        document.getElementById('filtroStatus').value = filtros.status || '';
        document.getElementById('filtroPrioridade').value = filtros.prioridade || '';
        filtrarMedicamentos();
    }
}

// Auto-save dos filtros (opcional)
setInterval(salvarFiltros, 5000); // Salva a cada 5 segundos

// Carregar filtros ao inicializar (descomente se desejado)
// window.addEventListener('load', carregarFiltros);