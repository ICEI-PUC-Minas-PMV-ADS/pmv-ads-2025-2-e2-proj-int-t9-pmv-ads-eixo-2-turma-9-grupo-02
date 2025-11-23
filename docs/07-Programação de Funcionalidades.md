# Programação de Funcionalidades

| ID     | Descrição do Requisito | Artefatos Produzidos | Aluno(a) Responsável |
|--------|-------------------------|-----------------------|------------------------|
| **RF-01** | A aplicação deve permitir que doadores (pessoa física) e instituições (pessoa jurídica) cadastrem seus perfis. | **Controllers:** CadastroController.cs, AuthController.cs <br> **Models:** Doador.cs, Instituição.cs, Usuario.cs, ContasViewModel.cs, AppDbContext.cs <br> **Views – Cadastro:** CreateDoador.cshtml, CreateDoador.css, CreateInstituicao.cshtml, CreateInstituicao.css, EditarDoador.cshtml, EditarInstituicao.cshtml <br> **Views – Auth:** ChooseType.cshtml, ChooseType.css, ChooseTypeRegister.cshtml <br> **Banco de Dados:** Tabelas Doadores, Instituicoes, Usuarios | Jefferson Torres |
| **RF-02** | A aplicação deve autenticar doadores e instituições por meio de login e senha. | **Controllers:** AuthController.cs <br> **Models:** Usuario.cs, ContasViewModel.cs, AppDbContext.cs <br> **Views – Auth:** Login.cshtml, Login.css <br> **Views Auxiliares:** _ValidationScriptsPartial.cshtml, _Layout.cshtml <br> **Banco de Dados:** Tabela Usuarios | Jefferson Torres |
| **RF-03** | O doador deve cadastrar medicamentos informando nome, validade, quantidade, foto e receita digitalizada. | **Controllers:** DoacoesController.cs <br> **Models:** Doação.cs, StatusDoacao.cs, Doador.cs <br> **Views – Doacoes:** Create.cshtml, Details.cshtml, Edit.cshtml, Delete.cshtml, Index.cshtml <br> **Banco de Dados:** Tabela Doacoes | Maria Luisa Greco |
| **RF-04** | O doador deve selecionar uma instituição específica para doar o medicamento. | **Controllers:** DoacoesController.cs <br> **Models:** Doação.cs, Instituição.cs, Doador.cs <br> **Views – Doacoes:** Create.cshtml (dropdown de instituições) <br> **Banco de Dados:** Relação Doacoes → Instituicoes | Maria Luisa Greco |
| **RF-05** | O doador deve acompanhar o status das suas doações em uma tela dedicada (“Minhas Doações”). | **Controllers:** DoacoesController.cs <br> **Models:** Doação.cs, StatusDoacao.cs, Doador.cs <br> **Views – Doacoes:** Index.cshtml, Details.cshtml <br> **Views – Home:** HomePageDoador.cshtml <br> **Banco de Dados:** Tabela Doacoes (campo status) | Maria Luisa Greco |
| **RF-06** | A instituição deve publicar lista de medicamentos em falta. |**Controllers:** MedicamentosInstituicaoController.cs, DoacoesController.cs <br> **Models:** MedicamentoInstituicao.cs, Instituicao.cs, Doacao.cs, AppDbContext.cs <br> **Views** – MedicamentosInstituicao: Index.cshtml,Escassez.cshtml, Create.cshtml, Edit.cshtml, Details.cshtml, Delete.cshtml<br> **Views** – Doações: Create.cshtml <br> **Banco de Dados:** MedicamentosInstituicao, Instituicoes, Doacoes.  | Thaís Souto |
| **RF-07** | A instituição deve visualizar a lista de doações disponíveis com informações do doador, medicamento, validade e receita. | | Lavínia Santos |
| **RF-08** | A instituição deve aprovar ou rejeitar doações em até 24 horas. || Lavínia Santos |
| **RF-09** | A aplicação deve enviar notificações automáticas e lembretes para o doador e a instituição sobre etapas críticas do processo, incluindo decisão de aceite ou recusa e prazos de entrega/análise. | | Estevão Dias |
| **RF-10** | A instituição deve confirmar o recebimento dos medicamentos no painel. || Estevão Dias |
| **RF-11** | A aplicação deve disponibilizar um histórico de doações, listando doações feitas pelo doador e doações recebidas pela instituição. | | Higor Conceição |
| **RF-12** | A aplicação deve gerar relatórios de impacto com base no histórico de doações. || Higor Conceição |


# Instruções de acesso

**Link da aplicação disponível para acesso:** https://hmgprojectmedshare.azurewebsites.net/

Se houver usuário de teste, o login e a senha também deverão ser informados aqui (por exemplo: usuário - admin / senha - admin).

O link e o usuário/senha descritos acima são apenas exemplos de como tais informações deverão ser apresentadas.
