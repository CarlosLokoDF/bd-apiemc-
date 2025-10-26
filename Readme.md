# API HardwareStore (.NET 9)

![.NET](https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-11.0-239120?style=for-the-badge&logo=c-sharp)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=for-the-badge&logo=sqlite)

Projeto de API REST para gerenciamento de componentes de hardware, desenvolvido como atividade acadêmica para a disciplina de Desenvolvimento de Sistemas.

O sistema implementa um CRUD (Create, Read, Update, Delete) completo que funciona simultaneamente de duas formas:
1.  **API REST:** Endpoints para serem consumidos por aplicações externas (Ex: Postman, Swagger).
2.  **CLI (Terminal):** Um menu de console interativo para gerenciar o banco diretamente.

---

## :sparkles: Features

* **CRUD Completo:** Funcionalidades de Criar, Ler, Atualizar e Deletar componentes.
* **Modo Duplo (API + CLI):** A API e o menu do terminal rodam simultaneamente com um único comando (`dotnet run`).
* **Banco de Dados:** Persistência de dados com EF Core 9 e SQLite.
* **Migrations Automáticas:** O banco (`hardware.db`) é criado e as migrations são aplicadas automaticamente ao iniciar a aplicação.
* **Validação de Dados:**
    * **Na API:** Uso de `[DataAnnotations]` (retornando `400 Bad Request`) e tratamento de erros de negócio.
    * **No Banco:** O campo `Sku` (código do produto) é configurado como **índice único** para evitar duplicatas.
* **Tratamento de Erros:** Retorno de códigos HTTP corretos (`200`, `201`, `204`, `400`, `404`, `409`).

## :computer: Stack de Tecnologias

* **C# / .NET 9:** Plataforma de desenvolvimento.
* **ASP.NET Core Web API:** Para a construção dos endpoints REST.
* **Entity Framework Core 9:** ORM para manipulação do banco de dados.
* **SQLite:** Banco de dados local em arquivo.

---

## :rocket: Como Rodar o Projeto

Este guia assume que você já possui o **VS Code** e o **Postman** instalados.

### 1. Pré-requisitos (Configuração do Ambiente)

Você **precisa** ter duas coisas instaladas na sua máquina:

1.  **[.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)**: A plataforma para rodar o projeto.
2.  **Ferramenta EF Core CLI:** Essencial para gerenciar as migrations.

    ```bash
    dotnet tool install --global dotnet-ef
    ```

    > **:warning: Importante:** Se você instalar o `dotnet-ef` e o comando `dotnet ef` falhar com o erro "não foi encontrado", **REINICIE O VS CODE** (ou o seu terminal). O terminal precisa recarregar o PATH do sistema para encontrar o novo comando.

### 2. Executando a Aplicação

1.  Clone este repositório:
    ```bash
    git clone [URL_DO_SEU_REPOSITORIO]
    ```

2.  Navegue até a pasta do projeto:
    ```bash
    cd HardwareStore/HardwareStore
    ```

3.  Execute o projeto:
    ```bash
    dotnet run
    ```

A aplicação fará duas coisas:
1.  **Criar o banco:** O arquivo `hardware.db` aparecerá na pasta e as migrations serão aplicadas.
2.  **Iniciar a API e o CLI:** O terminal mostrará a URL da API (ex: `http://localhost:5242`) e o menu do console.

---

## :keyboard: Como Usar o Sistema

Você pode interagir com os dados de duas formas simultâneas:

### Modo 1: API (via Postman ou Swagger)

A API estará rodando na URL indicada (ex: `http://localhost:5242`).
A documentação interativa do Swagger pode ser acessada em `http://localhost:5242/swagger`.

### Modo 2: CLI (via Terminal)

No mesmo terminal onde você executou `dotnet run`, o menu interativo estará disponível: