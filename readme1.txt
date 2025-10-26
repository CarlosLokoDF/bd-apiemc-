Projeto API HardwareStore (.NET 9)

API REST para gerenciamento de componentes de hardware, desenvolvido como atividade acadêmica para a disciplina de Desenvolvimento de Sistemas.

O sistema implementa um CRUD completo que funciona simultaneamente de duas formas:
1.  API REST: Endpoints para serem consumidos por aplicações externas (Ex: Postman).
2.  CLI (Terminal): Um menu de console interativo para gerenciar o banco.

=========================
Stack Utilizada
=========================
* C# / .NET 9
* ASP.NET Core Web API
* Entity Framework Core 9
* SQLite

=========================
Features
=========================
* API REST completa com endpoints GET, GET(ID), POST, PUT, e DELETE.
* CLI (Terminal) com menu interativo para Cadastrar, Listar, Atualizar e Remover componentes.
* API e CLI rodam simultaneamente com um único comando (dotnet run).
* Banco de Dados SQLite (hardware.db) gerenciado via EF Core Migrations.
* Validações de dados na API (DataAnnotations) e tratamento de erros (400, 404, 409).
* Regra de Negócio: O campo Sku (código do produto) é definido como único no banco de dados.

=========================
Entidade (Model): HardwareComponent
=========================

Campo         | Tipo          | Restrições
---------------------------------------------------------------------
Id            | int           | Chave Primária (Auto-incremento)
Sku           | string(20)    | Obrigatório, Único (Ex: "NV-RTX5090")
Name          | string(100)   | Obrigatório
ComponentType | string(50)    | Obrigatório (Ex: "GPU", "CPU", "RAM")
Price         | decimal(18,2) | Obrigatório, Range (0.01+)
AddedAt       | DateTime      | Gerado automaticamente na criação (UTC)

=========================
Como Rodar o Projeto
=========================

1.  Clone este repositório:
    git clone [URL_DO_SEU_REPOSITORIO]
    cd HardwareStore/HardwareStore

2.  Rode o projeto:
    dotnet run

3.  Importante: O Program.cs está configurado para aplicar as migrations (criar o banco hardware.db) automaticamente ao iniciar. Você não precisa rodar "dotnet ef database update".

4.  O terminal mostrará duas coisas:
    * A API rodando (ex: API online em http://localhost:5242...)
    * O menu do CLI (ex: == HardwareStore CLI ==...)

=========================
Como Usar
=========================

Você pode interagir com o sistema de duas formas ao mesmo tempo:

Opção 1: Terminal (CLI)
-------------------------
Ao rodar "dotnet run", o menu interativo aparecerá no seu terminal.

== HardwareStore CLI ==
Console + API executando juntos!

Escolha uma opção:
1 - Cadastrar Componente
2 - Listar Componentes
3 - Atualizar Componente (por Id)
4 - Remover Componente (por Id)
0 - Sair
>

Opção 2: API (Postman)
-------------------------
A API estará disponível na URL indicada (ex: http://localhost:5242). Você pode usar o Postman para testar os endpoints.

=========================
Rotas (Endpoints) da API
=========================

Método | Rota               | Descrição
---------------------------------------------------------------------
POST   | /api/hardware      | Cria um novo componente.
GET    | /api/hardware      | Lista todos (filtra por ?type=...).
GET    | /api/hardware/{id} | Busca um componente pelo ID.
PUT    | /api/hardware/{id} | Atualiza um componente.
DELETE | /api/hardware/{id} | Remove um componente.

=========================
Exemplos de Requisição (JSON para Postman)
=========================

POST /api/hardware (Headers: Content-Type: application/json)
{
  "sku": "AMD-R9-9950X",
  "name": "AMD Ryzen 9 9950X",
  "componentType": "CPU",
  "price": 5499.90
}


PUT /api/hardware/1 (Headers: Content-Type: application/json)
{
  "id": 1,
  "sku": "AMD-R9-9950X",
  "name": "AMD Ryzen 9 9950X (Revisado)",
  "componentType": "CPU",
  "price": 5399.00
}