
# Metodologia

A metodologia adotada para o desenvolvimento do MedShare segue uma abordagem ágil, baseada no Scrum, com o apoio do Jira para gestão das atividades. O objetivo é garantir organização, colaboração entre os integrantes e entregas incrementais ao longo da disciplina.

## Controle de Versão

A ferramenta de controle de versão adotada no projeto foi o
[Git](https://git-scm.com/), sendo que o [Github](https://github.com)
foi utilizado para hospedagem do repositório.

O projeto segue a seguinte convenção para o nome de branches:

- `main`: versão estável já testada do software
- `unstable`: versão já testada do software, porém instável
- `testing`: versão em testes do software
- `dev`: versão de desenvolvimento do software

Quanto à gerência de issues, o Jira será usado como ferramenta principal para criação e acompanhamento de tarefas, mas etiquetas adicionais podem ser usadas no GitHub quando necessário:

- `documentation`: melhorias ou acréscimos à documentação
- `bug`: funcionalidade com problemas
- `enhancement`: melhoria de uma funcionalidade existente
- `feature`: inclusão de uma nova funcionalidade

Além disso:

- `Commits` seguem mensagens padronizadas, curtas e objetivas ( `ex.: feat: cadastro de doador `).

- `Merges` para a  `main ` só ocorrem após revisão de pelo menos um integrante via `pull request`.

-  `Tags` são utilizadas para marcar versões significativas (ex.:  `v1.0.0 `).

## Gerenciamento de Projeto

### Divisão de Papéis

Divisão de Papéis

- `Product Owner (PO)`: responsável por priorizar o **backlog** e alinhar requisitos. (Lavínia Santos)
- `Scrum Master`: garante a aplicação da metodologia e facilita a comunicação. (Maria Luisa Greco)
- `Time de Desenvolvimento`: responsável por implementar, testar e documentar as funcionalidades. (Estevão Dias, Higor Eduardo, Jefferson Torres, Lavínia Santos, Maria Luisa Greco e Thaís Souto.)

### Processo

O projeto segue ciclos curtos de desenvolvimento (**sprints de duas semanas**), com as seguintes etapas:

`Sprint Planning `: definição das tarefas com base no **Product Backlog (Jira)**.

 `Daily (acompanhamento) `: comunicação rápida via **WhatsApp/Teams** para alinhar atividades e bloqueios.

 `Sprint Review `: apresentação das entregas ao final de cada **sprint.**

 `Retrospectiva `: discussão sobre pontos de melhoria para o próximo ciclo.

O gerenciamento é realizado com Jira (**Kanban e Scrum**), organizado nas colunas: Backlog, To Do, In Progress, In Review e Done.

### Ferramentas

As ferramentas empregadas no projeto são:

Editor de Código: Visual Studio Code — escolhido pela integração com GitHub e suporte a extensões.

- `Visual Studio (VS)`: utilizado principalmente para desenvolvimento da aplicação back-end em .NET, aproveitando o suporte completo a C# e integração com depuração e testes.

- `Visual Studio Code (VS Code)`: utilizado para desenvolvimento de front-end (HTML, CSS, JavaScript/React) e edição de arquivos de configuração, com suporte a extensões e integração com GitHub.

- `Versionamento`: GitHub — central para controle de código e revisão por **pull requests**.

- `Gerenciamento de Projeto`: **Jira** — utilizado para organizar backlog, sprints, tarefas e acompanhar o progresso da equipe.

- `Comunicação`: WhatsApp e Teams — para alinhamento rápido da equipe.

- `Wireframing e Protótipos`: Figma — utilizado para a criação da interface e validação visual.

- `Modelagem`: Astah e Draw.io — utilizados para diagramas **UML** e **BPMN.**

O editor de código foi escolhido por oferecer integração com o sistema de versão. As ferramentas de comunicação utilizadas possuem integração semelhante e foram selecionadas por facilitar o acompanhamento do progresso. Por fim, Astah, Draw.io e Figma foram escolhidos por melhor captar as necessidades de prototipagem e modelagem da solução.
