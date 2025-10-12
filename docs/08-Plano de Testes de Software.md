# Plano de Testes de Software

Este documento define a estratégia e casos de teste para validar os requisitos funcionais da plataforma MedShare, a qual conecta doadores de medicamentos a instituições de saúde verificadas. O seu escopo gira em torno de testes para validação dos requisitos funcionais descritos na especificação deste projeto.

Os requisitos para realização dos testes de software são:

<ul><li>Plataforma publicada na internet;</li>
<li>Navegadores da internet: Chrome, Firefox ou Edge;</li>
<li>Banco de dados com medicamentos e instituições cadastradas;</li>
<li>Usuários teste (doadores e instituições) disponíveis.</li>
</ul>
 
| **Caso de Teste** 	| **CT01.01 – Cadastro de doador** 	|
|:---:	|:---:	|
|	Requisito Associado 	| RF-01 - A aplicação deve permitir que doadores (pessoa física) e instituições (pessoa jurídica) cadastrem seus perfis. |
| Objetivo do Teste 	| Verificar se o sistema permite o cadastro completo de um doador. |
| Passos 	| - Acessar a aplicação; <br> - Clicar em "Cadastro de doador";<br> - Preencher todos os campos obrigatórios com dados válidos (e-mail, nome, sobrenome, celular, CPF, data de nascimento, endereço, senha, confirmação de senha); <br> - Aceitar os termos de uso; <br> - Clicar em "Cadastrar". |
|Critério de Êxito | - O cadastro foi realizado com sucesso. |
|  	|  	|
| **Caso de Teste** 	| **CT01.02 – Cadastro de instituição**	|
|Requisito Associado | RF-01 - A aplicação deve permitir que doadores (pessoa física) e instituições (pessoa jurídica) cadastrem seus perfis. |
| Objetivo do Teste 	|  Verificar se o sistema permite o cadastro completo de uma instituição. |
| Passos 	|  - Acessar a aplicação; <br> - Clicar em "Cadastro de instituição";<br> - Preencher todos os campos obrigatórios com dados válidos (e-mail, razão social, telefone, CNPJ, endereço, senha, confirmação de senha); <br> - Aceitar os termos de uso; <br> - Clicar em "Cadastrar". |
|Critério de Êxito | - O cadastro foi realizado com sucesso. |
| **Caso de Teste** 	| **CT02 – Login com credenciais válidas**	|
|Requisito Associado | RF-02 - A aplicação deve autenticar doadores e instituições por meio de login e senha. |
| Objetivo do Teste 	| Verificar se doadores e instituições conseguem acessar o sistema com as credenciais cadastradas. |
| Passos 	| - Acessar a página de login da aplicação; <br> - Inserir e-mail e senha válidos; <br> - Clicar em "Entrar". |
|Critério de Êxito | - O login foi realizado com sucesso. |
| **Caso de Teste** 	| **CT03 – Cadastro de medicamentos**	|
|Requisito Associado | RF-03 - O doador deve cadastrar medicamentos informando nome, validade, quantidade, foto e receita digitalizada (quando aplicável). |
| Objetivo do Teste 	| Verificar se doadores conseguem cadastrar medicamentos no sistema. |
| Passos 	| - Fazer login com dados de um doador;<br> - Clicar em "Cadastrar medicamento";<br> - Preencher todos os campos obrigatórios com dados válidos (nome, validade, quantidade, receita); <br> - Clicar em "Cadastrar medicamento". |
|Critério de Êxito | - O cadastro de medicamento foi realizado com sucesso. |
| **Caso de Teste** 	| **CT04 – Visualização de instituições próximas**	|
|Requisito Associado | RF-04 - O doador deve visualizar instituições próximas em um mapa com base na geolocalização. |
| Objetivo do Teste 	| Verificar se a geolocalização mostra instituições próximas corretamente. |
| Passos 	| - Fazer login com dados de um doador;<br> - Clicar em "Ver instituições próximas";<br> - Permitir à aplicação o acesso à localização de seu dispositivo;<br> - Visualizar a lista de instiuições próximas à localização do usuário. |
|Critério de Êxito | - A visualização da lista de instituições próximas foi realizada com sucesso. |
| **Caso de Teste** 	| **CT05 – Seleção de instituição para doação do medicamento**	|
|Requisito Associado | RF-05 - O doador deve selecionar uma instituição específica para doar o medicamento. |
| Objetivo do Teste 	| Verificar se a geolocalização mostra instituições próximas corretamente. |
| Passos 	| - Fazer login com dados de um doador;<br> - Realizar o cadastro de um medicamento;<br> - Selecionar um medicamento para doar;<br> - Clicar em "Escolher instituição"; <br> - Escolher uma instituição no mapa. <br> - Clicar em "Criar solicitação".|
|Critério de Êxito | - A seleção de uma instituição para a doação de medicamento foi realizada com sucesso. |
| **Caso de Teste** 	| **CT06 – Acompanhamento de status das doações**	|
|Requisito Associado | RF-06 - 	O doador deve acompanhar o status das suas doações em uma tela dedicada ("Minhas Doações"). |
| Objetivo do Teste 	| Verificar se doador visualiza o status correto das doações. |
| Passos 	| - Fazer login com dados de um doador;<br> - Clicar em "Ver solicitações";<br> - Verificar se a aplicação exibe o status da solicitação (Aprovado, Pendente ou Rejeitado). |
|Critério de Êxito | - A vizualização do status da doação foi realizada com sucesso. |
| **Caso de Teste** 	| **CT07 – Listagem de medicamentos prioritários**	|
|Requisito Associado | RF-07 - 	A instituição deve listar medicamentos prioritários em status “Escassez critica” visível no painel. |
| Objetivo do Teste 	| Verificar a listagem de medicamentos prioritários para instituições. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Estoque de medicamentos";<br> - Clicar em "Ver medicamentos e quantidades";<br> - Selecionar os medicamentos que deseja listar como prioritários; <br> - Clicar em "Salvar". |
|Critério de Êxito | - A listagem de medicamentos prioritários foi realizada com sucesso. |
| **Caso de Teste** 	| **CT08 – Visualização de doações disponíveis**	|
|Requisito Associado | RF-08 - 	A instituição deve visualizar a lista de doações disponíveis com informações do doador, medicamento, validade e receita. |
| Objetivo do Teste 	| Verificar se instituições visualizam doações pendentes. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Todas as solicitações";<br> - Clicar em "Ver todas as solicitações".<br> |
|Critério de Êxito | - A visualização das doações disponíveis foi realizada com sucesso. |
| **Caso de Teste** 	| **CT09 – Aprovação/Rejeição de doações**	|
|Requisito Associado | RF-09 - 	A instituição deve aprovar ou rejeitar doações em até 24 horas. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Todas as solicitações"; <br> - Clicar em "Ver todas as solicitações";<br> - Clicar em "Aprovar doação" ou "Rejeitar doação"; <br> - Confirmar sua escolha. |
|Critério de Êxito | - A aprovação ou rejeição da doação foi realizada com sucesso. |
| **Caso de Teste** 	| **CT10.01 – Notificações do processo de doação**	|
|Requisito Associado | RF-10 - 	A aplicação deve enviar notificações automáticas e lembretes para o doador e a instituição sobre etapas críticas do processo, incluindo decisão de aceite ou recusa e prazos de entrega/análise. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| - Doador realiza uma solicitação de um medicamento;<br> - Fazer login com dados de uma instituição;<br> - Clicar na aba "Notificações"; <br> - Visualizar notificações sobre processos de doação. |
|Critério de Êxito | - A notificação sobre o processo de doação foi realizada com sucesso. |
| **Caso de Teste** 	| **CT10.02 – Notificações do processo de doação**	|
|Requisito Associado | RF-10 - 	A aplicação deve enviar notificações automáticas e lembretes para o doador e a instituição sobre etapas críticas do processo, incluindo decisão de aceite ou recusa e prazos de entrega/análise. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| - Instituição aprova/rejeita doação;<br> - Fazer login com dados de um doador;<br> -  Clicar na aba "Notificações";<br> - Visualizar notificações sobre processos de doação. |
|Critério de Êxito | - A notificação sobre o processo de doação foi realizada com sucesso. |
| **Caso de Teste** 	| **CT11 – Confirmação de recebimento de medicamentos**	|
|Requisito Associado | RF-11 - 	A instituição deve confirmar o recebimento dos medicamentos no painel. |
| Objetivo do Teste 	| Verificar se a instituição consegue confirmar o recebimento de medicamentos. |
| Passos 	| - Doação de medicamento é recebida pela instituição;<br> - Fazer login com dados de uma instituição;<br> -  Clicar em "Ver todas as solicitações";<br> - Visualizar as solicitações aprovadas;<br> - Clicar em "Confirmar recebimento". |
|Critério de Êxito | - A confirmação de recebimento do medicamento foi realizada com sucesso. |
| **Caso de Teste** 	| **CT12 – Histórico de doações**	|
|Requisito Associado | RF-12 - 	A aplicação deve disponibilizar um histórico de doações, listando doações feitas pelo doador e doações recebidas pela instituição. |
| Objetivo do Teste 	| Verificar se é possível visualizar o histórico de doações feitas/recebidas. |
| Passos 	| Realização de doações entre doador e instituição; <br> - Fazer login com dados de uma instituição ou de um doador;<br> - Clicar em "Histórico de doações". |
|Critério de Êxito | - A visualização do histórico de doações foi realizada com sucesso. |
| **Caso de Teste** 	| **CT13 – Relatório de impacto**	|
|Requisito Associado | RF-13 - 	A aplicação deve gerar relatórios de impacto com base no histórico de doações. |
| Objetivo do Teste 	| Verificar se a aplicação gera relatórios de impacto com base no histórico de doações realizadas. |
| Passos 	| Realização de doações entre doador e instituição; <br> - Fazer login com dados de uma instituição ou de um doador;<br> - Clicar em "Histórico de doações"; <br> - Clicar em "Relatório de impacto". |
|Critério de Êxito | - A geração do relatório de impacto foi realizada com sucesso. |
