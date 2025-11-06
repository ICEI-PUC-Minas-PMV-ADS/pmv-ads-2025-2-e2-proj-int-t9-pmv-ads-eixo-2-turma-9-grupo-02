// ================================================
// JAVASCRIPT PARA CADASTRO DE MEDICAMENTOS
// ================================================

document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('formCadastro');
    const quantidadeNecessaria = document.getElementById('quantidadeNecessaria');
    const quantidadeAtual = document.getElementById('quantidadeAtual');
    const statusBadge = document.getElementById('statusBadge');
    const statusProgress = document.getElementById('statusProgress');
    const statusPercentual = document.getElementById('statusPercentual');

    // Event listeners para preview do status
    quantidadeNecessaria.addEventListener('input', atualizarPreview);
    quantidadeAtual.addEventListener('input', atualizarPreview);

    // Event listener para o formulário
    form.addEventListener('submit', handleSubmit);

    // Validação em tempo real
    const campos = form.querySelectorAll('input, select, textarea');
    campos.forEach(campo => {
        campo.addEventListener('blur', validarCampo);
        campo.addEventListener('input', function() {
            if (campo.classList.contains('is-invalid')) {
                validarCampo.call(this);
            }
        });
    });

    // Preview inicial
    atualizarPreview();
});

// Função para atualizar preview do status
function atualizarPreview() {
    const necessaria = parseInt(document.getElementById('quantidadeNecessaria').value) || 0;
    const atual = parseInt(document.getElementById('quantidadeAtual').value) || 0;
    
    const statusBadge = document.getElementById('statusBadge');
    const statusProgress = document.getElementById('statusProgress');
    const statusPercentual = document.getElementById('statusPercentual');

    if (necessaria === 0) {
        statusBadge.textContent = '⚠️ Defina a quantidade necessária';
        statusBadge.className = 'badge bg-warning';
        statusProgress.style.width = '0%';
        statusProgress.className = 'progress-bar bg-secondary';
        statusPercentual.textContent = '0%';
        return;
    }

    const porcentagem = (atual / necessaria) * 100;
    const porcentagemArredondada = Math.round(porcentagem * 10) / 10;
    
    // Determinar status
    let status, classe, icone, corProgress;
    if (atual < (necessaria * 0.3)) {
        status = 'Escassez Crítica';
        classe = 'badge bg-danger';
        icone = '🚨';
        corProgress = 'progress-bar bg-danger';
    } else if (porcentagem < 50) {
        status = 'Estoque Baixo';
        classe = 'badge bg-warning';
        icone = '⚠️';
        corProgress = 'progress-bar bg-warning';
    } else {
        status = 'Estoque Normal';
        classe = 'badge bg-success';
        icone = '✅';
        corProgress = 'progress-bar bg-success';
    }

    statusBadge.textContent = `${icone} ${status}`;
    statusBadge.className = classe;
    statusProgress.className = corProgress;
    statusProgress.style.width = `${Math.min(porcentagem, 100)}%`;
    statusPercentual.textContent = `${porcentagemArredondada}%`;
}

// Função para validar campo individual
function validarCampo() {
    const campo = this;
    const valor = campo.value.trim();
    let valido = true;

    // Remover classes anteriores
    campo.classList.remove('is-valid', 'is-invalid');

    // Validações específicas por campo
    switch (campo.name) {
        case 'nome':
            valido = valor.length >= 2;
            break;
        case 'categoria':
        case 'prioridade':
            valido = valor !== '';
            break;
        case 'dosagem':
            valido = valor.length >= 1;
            break;
        case 'quantidadeNecessaria':
            valido = valor !== '' && parseInt(valor) > 0;
            break;
        case 'quantidadeAtual':
            valido = valor === '' || parseInt(valor) >= 0;
            break;
        default:
            // Para campos opcionais
            valido = true;
    }

    // Aplicar classe de validação
    if (campo.hasAttribute('required') && !valido) {
        campo.classList.add('is-invalid');
    } else if (campo.hasAttribute('required') || valor !== '') {
        campo.classList.add('is-valid');
    }

    return valido;
}

// Função para validar formulário completo
function validarFormulario() {
    const form = document.getElementById('formCadastro');
    const campos = form.querySelectorAll('input[required], select[required]');
    let formularioValido = true;

    campos.forEach(campo => {
        if (!validarCampo.call(campo)) {
            formularioValido = false;
        }
    });

    return formularioValido;
}

// Função para lidar com o envio do formulário
function handleSubmit(event) {
    event.preventDefault();
    
    // Validar formulário
    if (!validarFormulario()) {
        // Scroll para o primeiro campo inválido
        const primeiroInvalido = document.querySelector('.is-invalid');
        if (primeiroInvalido) {
            primeiroInvalido.scrollIntoView({ behavior: 'smooth', block: 'center' });
            primeiroInvalido.focus();
        }
        return;
    }

    // Coletar dados do formulário
    const formData = new FormData(document.getElementById('formCadastro'));
    const dados = Object.fromEntries(formData);

    // Adicionar dados calculados
    dados.dataCadastro = new Date().toISOString();
    dados.ultimaAtualizacao = new Date().toISOString();
    dados.ativo = true;

    // Simular salvamento
    mostrarLoading(true);

    setTimeout(() => {
        mostrarLoading(false);
        
        // Simular sucesso
        mostrarMensagemSucesso(dados);
        
        // Opcional: redirecionar após delay
        setTimeout(() => {
            if (confirm('Medicamento cadastrado com sucesso! Deseja voltar à lista?')) {
                window.location.href = 'index.html';
            } else {
                // Limpar formulário para novo cadastro
                document.getElementById('formCadastro').reset();
                document.querySelectorAll('.is-valid, .is-invalid').forEach(el => {
                    el.classList.remove('is-valid', 'is-invalid');
                });
                atualizarPreview();
            }
        }, 2000);
        
    }, 2000);
}

// Função para mostrar loading
function mostrarLoading(mostrar) {
    const btnSubmit = document.querySelector('button[type="submit"]');
    const form = document.getElementById('formCadastro');
    
    if (mostrar) {
        btnSubmit.disabled = true;
        btnSubmit.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Salvando...';
        form.classList.add('loading');
    } else {
        btnSubmit.disabled = false;
        btnSubmit.innerHTML = '<i class="fas fa-save me-2"></i>Cadastrar Medicamento';
        form.classList.remove('loading');
    }
}

// Função para mostrar mensagem de sucesso
function mostrarMensagemSucesso(dados) {
    // Criar toast ou alert de sucesso
    const toast = document.createElement('div');
    toast.className = 'alert alert-success alert-dismissible fade show position-fixed';
    toast.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    toast.innerHTML = `
        <i class="fas fa-check-circle me-2"></i>
        <strong>Sucesso!</strong> Medicamento "${dados.nome}" cadastrado com sucesso.
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(toast);
    
    // Auto-remover após 5 segundos
    setTimeout(() => {
        if (toast.parentNode) {
            toast.remove();
        }
    }, 5000);
}

// Função para auto-completar baseado no nome (opcional)
function autoCompletarMedicamento() {
    const nome = document.getElementById('nome').value.toLowerCase();
    
    // Banco de dados básico de medicamentos conhecidos
    const medicamentosConhecidos = {
        'paracetamol': {
            categoria: 'Analgésicos',
            formaFarmaceutica: 'Comprimido',
            dosagem: '500mg'
        },
        'ibuprofeno': {
            categoria: 'Anti-inflamatórios',
            formaFarmaceutica: 'Comprimido',
            dosagem: '400mg'
        },
        'amoxicilina': {
            categoria: 'Antibióticos',
            formaFarmaceutica: 'Cápsula',
            dosagem: '500mg'
        }
        // Adicionar mais medicamentos conforme necessário
    };

    const medicamento = medicamentosConhecidos[nome];
    if (medicamento) {
        if (confirm(`Detectamos informações para ${nome}. Deseja preenchê-las automaticamente?`)) {
            document.getElementById('categoria').value = medicamento.categoria;
            document.getElementById('formaFarmaceutica').value = medicamento.formaFarmaceutica;
            if (!document.getElementById('dosagem').value) {
                document.getElementById('dosagem').value = medicamento.dosagem;
            }
        }
    }
}

// Event listener para auto-completar
document.getElementById('nome').addEventListener('blur', autoCompletarMedicamento);

// Função para salvar rascunho no localStorage
function salvarRascunho() {
    const form = document.getElementById('formCadastro');
    const formData = new FormData(form);
    const rascunho = Object.fromEntries(formData);
    
    localStorage.setItem('rascunhoMedicamento', JSON.stringify(rascunho));
}

// Função para carregar rascunho
function carregarRascunho() {
    const rascunho = localStorage.getItem('rascunhoMedicamento');
    if (rascunho) {
        const dados = JSON.parse(rascunho);
        
        if (confirm('Encontramos um rascunho salvo. Deseja carregá-lo?')) {
            Object.keys(dados).forEach(campo => {
                const elemento = document.getElementById(campo);
                if (elemento) {
                    elemento.value = dados[campo];
                }
            });
            atualizarPreview();
        }
        
        // Limpar rascunho após carregar
        localStorage.removeItem('rascunhoMedicamento');
    }
}

// Auto-salvar rascunho a cada 30 segundos
setInterval(salvarRascunho, 30000);

// Carregar rascunho ao inicializar (descomente se desejado)
// window.addEventListener('load', carregarRascunho);