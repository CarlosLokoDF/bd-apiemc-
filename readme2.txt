README2.txt - Log de Desenvolvimento e Manual de Setup Completo

Este documento detalha todos os passos, desde a configuração inicial do ambiente de desenvolvimento até a finalização do projeto HardwareStore.
O objetivo é registrar o processo completo, incluindo ferramentas, instalação, troubleshooting (resolução de problemas) e a implementação do código.

================================================================
Seção 1: Configuração do Ambiente (Pré-requisitos)
================================================================

Antes de criar o projeto, o ambiente de desenvolvimento no Windows foi preparado com as seguintes ferramentas:

1. Visual Studio Code (VS Code)
   - Descrição: O editor de código principal utilizado para escrever todo o código C#.
   - Status: Já estava instalado.

2. Extensões do VS Code
   - Descrição: Foi instalada a extensão 'C# Dev Kit' (que inclui o 'C#' e o 'IntelliCode') da Microsoft para habilitar o autocompletar, depuração e gerenciamento de projetos .NET.

3. Postman
   - Descrição: Ferramenta utilizada para testar os endpoints da API REST (testar POST, GET, PUT, DELETE).
   - Status: Já estava instalado.

4. SDK do .NET 9
   - Descrição: A plataforma de desenvolvimento da Microsoft. Foi instalado o SDK (Software Development Kit) do .NET 9 para ser possível compilar e executar o projeto com a versão mais recente.
   - Status: Instalado durante o processo.

5. Ferramenta EF Core (dotnet-ef)
   - Descrição: Esta é uma ferramenta de linha de comando (CLI) essencial para que o Entity Framework Core possa criar as 'Migrations' (a "receita" do banco de dados).
   - Problema: Ao rodar 'dotnet ef migrations add...', o terminal retornou o erro "dotnet-ef não existe".
   - Solução: Foi necessário instalar a ferramenta globalmente com o comando:
     > dotnet tool install --global dotnet-ef
   - Troubleshooting (Resolução): Mesmo após a instalação, o comando continuou falhando no terminal integrado do VS Code.
     - Causa: O terminal não tinha atualizado o "PATH" (caminho) do sistema para reconhecer o novo comando.
     - Resolução Final: Foi necessário **reiniciar o VS Code**. Ao reabrir, o terminal carregou o PATH atualizado e o comando 'dotnet ef' passou a funcionar.

================================================================
Seção 2: Criação e Estrutura do Projeto "HardwareStore"
================================================================

1. Criação do Projeto
   - Usamos o terminal para criar um novo projeto de Web API:
     > dotnet new webapi -n HardwareStore
   - Entramos na pasta do projeto:
     > cd HardwareStore

2. Instalação de Pacotes (NuGet)
   - Adicionamos os pacotes necessários para o projeto:
     > dotnet add package Microsoft.EntityFrameworkCore
     > dotnet add package Microsoft.EntityFrameworkCore.Sqlite
     > dotnet add package Microsoft.EntityFrameworkCore.Design
     > dotnet add package Swashbuckle.AspNetCore (Para a documentação Swagger)

3. Criação do Banco de Dados (Migration)
   - Após corrigir o 'dotnet-ef' e criar os modelos, o banco foi criado com:
     > dotnet ef migrations add CreateHardwareStoreSchema
     > dotnet ef database update
   - (Nota: Posteriormente, automatizamos isso no Program.cs)

================================================================
Seção 3: Implementação do Código (Os 4 Arquivos Principais)
================================================================

1. Models/HardwareComponent.cs
   - Criamos o modelo da peça de hardware.
   - Usamos 'DataAnnotations' (ex: [Required], [StringLength]) para validações rápidas.

2. Data/AppDbContext.cs
   - Criamos o "contexto" do banco, que faz a ponte entre o C# e o SQLite.
   - Usamos 'OnModelCreating' para definir regras avançadas do banco (ex: 'HasIndex(c => c.Sku).IsUnique()').
   - Problema: Ao tentar rodar o menu do console, tivemos o erro "Não há nenhum argumento fornecido que corresponda ao parâmetro 'options'".
   - Solução: O DbContext precisava de *dois* construtores:
     1. `public AppDbContext(DbContextOptions<AppDbContext> options)`: Para a API (injeção de dependência).
     2. `public AppDbContext()`: Um construtor vazio para o menu do console (CLI).
   - Também adicionamos o método `OnConfiguring` para que o construtor vazio soubesse qual banco usar.

3. Controllers/HardwareController.cs
   - Implementamos a API REST (o "CRUD") neste arquivo.
   - Criamos os 5 endpoints (GET, GET por ID, POST, PUT, DELETE).
   - Adicionamos validações de erro 404 (Não Encontrado) e 409 (Conflito, para SKU duplicado).

4. Program.cs (O "Cérebro")
   - Este foi o arquivo mais modificado.
   - Configuramos a API (Swagger, Controllers, DbContext).
   - Implementamos a funcionalidade de "API + CLI" simultâneos:
     1. A API roda em segundo plano (`app.RunAsync()`).
     2. O menu do console (CLI) roda em um loop `while (true)`.
     3. Implementamos as funções do menu (CreateHardwareAsync, ListHardwareAsync, etc.) que usam `new AppDbContext()` para acessar o banco, exatamente como o projeto 'Escola'.

================================================================
Seção 4: Testes e Resolução de Problemas da API
================================================================

Após rodar o projeto (`dotnet run`), usamos o Postman para testar a API.

1. Erro 415 (Unsupported Media Type)
   - Causa: Ao tentar fazer um 'POST', o Postman não estava informando à API que o dado enviado era um JSON.
   - Solução: Adicionamos o 'Header' na requisição do Postman:
     - Key: Content-Type
     - Value: application/json

2. Erro 400 (Bad Request - "A non-empty request body is required")
   - Causa: Após corrigir o erro 415, a requisição estava sendo enviada com o Header correto, mas o 'Body' (Corpo) da requisição estava vazio.
   - Solução: Adicionamos o JSON do 'HardwareComponent' na aba 'Body' > 'raw' do Postman.

3. Sucesso (201 Created)
   - Após corrigir os erros 415 e 400, a requisição 'POST' funcionou e retornou '201 Created'.

4. Teste Completo
   - Testamos o 'GET' (listou o item criado), o 'PUT' (atualizou o item) e o 'DELETE' (removeu o item).
   - Também testamos o menu do console, confirmando que ele lia e escrevia no *mesmo* banco de dados que a API.

================================================================
Seção 5: Resultado Final
================================================================

O projeto foi finalizado com sucesso, atendendo a todos os requisitos: uma API REST funcional, um menu de console (CLI) funcional, persistência com EF Core e SQLite, e documentação completa (`README.txt` e este `README2.txt`).