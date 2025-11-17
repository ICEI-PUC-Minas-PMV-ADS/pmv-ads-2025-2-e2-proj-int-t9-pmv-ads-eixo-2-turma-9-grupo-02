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
|Critério de Êxito | O sistema salva o cadastro da instituição e redireciona automaticamente para a tela de login, sem exibir mensagens de erro. |

| **Caso de Teste** 	| **CT01.02 – Cadastro de instituição**	|
|:---:	|:---:	|
|Requisito Associado | RF-01 - A aplicação deve permitir que doadores (pessoa física) e instituições (pessoa jurídica) cadastrem seus perfis. |
| Objetivo do Teste 	|  Verificar se o sistema permite o cadastro completo de uma instituição. |
| Passos 	|  - Acessar a aplicação; <br> - Clicar em "Cadastro de instituição";<br> - Preencher todos os campos obrigatórios com dados válidos (e-mail, razão social, telefone, CNPJ, endereço, senha, confirmação de senha); <br> - Aceitar os termos de uso; <br> - Clicar em "Cadastrar". |
|Critério de Êxito | O sistema salva o cadastro da instituição e redireciona automaticamente para a tela de login, sem exibir mensagens de erro. |

| **Caso de Teste** 	| **CT02 – Login com credenciais válidas**	|
|:---:	|:---:	|
|Requisito Associado | RF-02 - A aplicação deve autenticar doadores e instituições por meio de login e senha. |
| Objetivo do Teste 	| Verificar se doadores e instituições conseguem acessar o sistema com as credenciais cadastradas. |
| Passos 	| - Acessar a página de login da aplicação; <br> - Inserir e-mail e senha válidos; <br> - Clicar em "Entrar". |
|Critério de Êxito | O sistema autentica o usuário e redireciona automaticamente para a Home (Home Doador ou Home Instituição), sem apresentar mensagens de erro. |

| **Caso de Teste** 	| **CT03 – Cadastro de medicamentos**	|
|:---:	|:---:	|
|Requisito Associado | RF-03 - O doador deve cadastrar medicamentos informando nome, validade, quantidade, foto e receita digitalizada (quando aplicável). |
| Objetivo do Teste 	| Verificar se doadores conseguem cadastrar medicamentos no sistema. |
| Passos 	| - Fazer login com dados de um doador;<br> - Clicar em "Cadastrar medicamento";<br> - Preencher todos os campos obrigatórios com dados válidos (nome, validade, quantidade, receita); <br> - Clicar em "Cadastrar medicamento". |
|Critério de Êxito | O sistema salva o medicamento e redireciona automaticamente para a tela “Minhas Doações”, onde o novo medicamento aparece listado. |

| **Caso de Teste** 	| **CT04 – Seleção de instituição para doação do medicamento**	|
|:---:	|:---:	|
|Requisito Associado | RF-04 - O doador deve selecionar uma instituição específica para doar o medicamento. |
| Objetivo do Teste 	| Verificar se a aplicação mostra uma lista com as instituições cadastradas. |
| Passos 	| - Fazer login com dados de um doador;<br> - Realizar o cadastro de um medicamento em "Nova Doação";<br> - Preencher os campos de cadastro;<br> - Clicar em "Selecionar uma instituição"; <br> - Escolher uma instituição na lista disponível. <br> - Clicar em "Cadastrar".|
|Critério de Êxito | O sistema vincula a instituição escolhida ao medicamento e exibe essa associação na tela “Minhas Doações”, sem mensagens de erro. |

| **Caso de Teste** 	| **CT05 – Acompanhamento de status das doações**	|
|:---:	|:---:	|
|Requisito Associado | RF-05 - 	O doador deve acompanhar o status das suas doações em uma tela dedicada ("Minhas Doações"). |
| Objetivo do Teste 	| Verificar se doador visualiza o status correto das doações. |
| Passos 	| - Fazer login com dados de um doador;<br> - Clicar em "Minhas Doações";<br> - Verificar se a aplicação exibe o status da solicitação (Pendente, Aprovado, Finalizado ou Rejeitado). |
|Critério de Êxito | O sistema exibe o status atualizado da doação diretamente na tela “Minhas Doações”. |

| **Caso de Teste** 	| **CT06 – Listagem de medicamentos em falta**	|
|:---:	|:---:	|
|Requisito Associado | RF-06 - 	A instituição deve publicar lista de medicamentos em falta. |
| Objetivo do Teste 	| Verificar a listagem de medicamentos em falta para instituições. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Estoque de medicamentos";<br> - Clicar em “Novo cadastro”;<br> - Preencher os campos obrigatórios; <br> - Clicar em "Salvar". |
|Critério de Êxito | O medicamento é salvo e, ao atingir a quantidade mínima, aparece automaticamente na lista de “Medicamentos em falta” quando o usuário pesquisa a instituição em “Buscar Instituições”. |

| **Caso de Teste** 	| **CT07 – Visualização de doações disponíveis**	|
|:---:	|:---:	|
|Requisito Associado | RF-07 - 	A instituição deve visualizar a lista de doações disponíveis com informações do doador, medicamento, validade e receita. |
| Objetivo do Teste 	| Verificar se instituições visualizam doações pendentes. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Todas as solicitações";<br> - Clicar em "Ver todas as solicitações".<br> |
|Critério de Êxito | As doações pendentes aparecem listadas corretamente na tela “Doações” da instituição, com todas as informações necessárias. |

| **Caso de Teste** 	| **CT08 – Aprovação/Rejeição de doações**	|
|:---:	|:---:	|
|Requisito Associado | RF-08 - 	A instituição deve aprovar ou rejeitar doações em até 24 horas. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| - Fazer login com dados de uma instituição;<br> - Clicar em "Todas as solicitações"; <br> - Clicar em "Ver todas as solicitações";<br> - Clicar em "Aprovar doação" ou "Rejeitar doação" ; <br> - Confirmar sua escolha. |
|Critério de Êxito | O status da doação é atualizado automaticamente para Aprovado ou Rejeitado e aparece atualizado na lista de solicitações e na tela "Minhas Doações" (para o doador). |

| **Caso de Teste** 	| **CT09.01 – Notificações do processo de doação**	|
|:---:	|:---:	|
|Requisito Associado | RF-09 - 	A aplicação deve enviar notificações automáticas e lembretes para o doador e a instituição sobre etapas críticas do processo, incluindo decisão de aceite ou recusa e prazos de entrega/análise. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| <br> - Fazer login com dados de um doador; <br> - Clicar na aba "Minhas Doações" e em seguida no botão "Nova Doação"; <br> - Doador realiza uma doação de um medicamento, selecionando uma instituição para doar;<br> - Clicar na aba "Notificações"; <br> - Visualizar notificações sobre processos de doação. |
|Critério de Êxito | As notificações aparecem automaticamente na lateral da tela do doador conforme a doação é criada e o status é atualizado. |
| **Caso de Teste** 	| **CT09.02 – Notificações do processo de doação**	|
|Requisito Associado | RF-09 - 	A aplicação deve enviar notificações automáticas e lembretes para o doador e a instituição sobre etapas críticas do processo, incluindo decisão de aceite ou recusa e prazos de entrega/análise. |
| Objetivo do Teste 	| Verificar se a instituição consegue aprovar ou rejeitar doações de medicamentos. |
| Passos 	| <br> - Fazer login com dados de uma instituição;<br> - Instituição aprova/rejeita doação;<br> -  Clicar na aba "Notificações";<br> - Visualizar notificações sobre processos de doação. |
|Critério de Êxito | As notificações são exibidas automaticamente na lateral da tela da instituição conforme o status das doações muda. |

| **Caso de Teste** 	| **CT10 – Confirmação de recebimento de medicamentos**	|
|:---:	|:---:	|
|Requisito Associado | RF-10 - 	A instituição deve confirmar o recebimento dos medicamentos no painel. |
| Objetivo do Teste 	| Verificar se a instituição consegue confirmar o recebimento de medicamentos. |
| Passos 	| - Doação de medicamento é recebida pela instituição;<br> - Fazer login com dados de uma instituição;<br> -  Clicar em "Ver todas as solicitações";<br> - Visualizar as solicitações aprovadas;<br> - Clicar em "Confirmar recebimento". |
|Critério de Êxito | O status da doação muda automaticamente para Finalizado, indicando o recebimento |

| **Caso de Teste** 	| **CT11 – Histórico de doações**	|
|:---:	|:---:	|
|Requisito Associado | RF-11 - 	A aplicação deve disponibilizar um histórico de doações, listando doações feitas pelo doador e doações recebidas pela instituição. |
| Objetivo do Teste 	| Verificar se é possível visualizar o histórico de doações feitas/recebidas. |
| Passos 	| Realização de doações entre doador e instituição; <br> - Fazer login com dados de uma instituição ou de um doador;<br> - Clicar em "Histórico de doações". |
|Critério de Êxito | O histórico aparece corretamente na tela “Minhas Doações” (para o doador) ou na tela “Doações” (para a instituição). |

| **Caso de Teste** 	| **CT12 – Relatório de impacto**	|
|:---:	|:---:	|
|Requisito Associado | RF-12 - 	A aplicação deve gerar relatórios de impacto com base no histórico de doações. |
| Objetivo do Teste 	| Verificar se a aplicação gera relatórios de impacto com base no histórico de doações realizadas. |
| Passos 	| Realização de doações entre doador e instituição; <br> - Fazer login com dados de uma instituição ou de um doador;<br> - Clicar em "Histórico de doações"; <br> - Clicar em "Relatório de impacto". |
|Critério de Êxito | O relatório de impacto é gerado automaticamente e exibido na tela, com base nos dados do histórico de doações. |
