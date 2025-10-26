using HardwareStore.Data;
using HardwareStore.Models; // Importa nosso modelo
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Configuração da API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=hardware.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// --- Fim da Configuração da API ---


// 1. Roda a API em segundo plano (como no seu projeto Escola)
var webTask = app.RunAsync("http://localhost:5242"); // Porta fixa facilita
Console.WriteLine("API online em http://localhost:5242 (Swagger em /swagger)");


// 2. Garante que as migrations foram aplicadas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// --- Início do Menu CLI (adaptado do seu projeto Escola) ---
Console.WriteLine("== HardwareStore CLI ==");
Console.WriteLine("Console + API executando juntos!");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Cadastrar Componente");
    Console.WriteLine("2 - Listar Componentes");
    Console.WriteLine("3 - Atualizar Componente (por Id)");
    Console.WriteLine("4 - Remover Componente (por Id)");
    Console.WriteLine("0 - Sair");
    Console.Write("> ");

    var opt = Console.ReadLine();
    if (opt == "0") break;

    // Usamos 'await' aqui
    switch (opt)
    {
        case "1": await CreateHardwareAsync(); break;
        case "2": await ListHardwareAsync(); break;
        case "3": await UpdateHardwareAsync(); break;
        case "4": await DeleteHardwareAsync(); break;
        default: Console.WriteLine("Opção inválida."); break;
    }
}

// Para a API graciosamente quando o loop do console terminar
await app.StopAsync();
await webTask; // Aguarda a API desligar


// --- Métodos do CLI (adaptados do seu projeto Escola) ---

async Task CreateHardwareAsync()
{
    Console.Write("SKU (Código): ");
    var sku = (Console.ReadLine() ?? "").Trim().ToUpper();

    Console.Write("Nome: ");
    var name = (Console.ReadLine() ?? "").Trim();
    
    Console.Write("Tipo (GPU, CPU, RAM...): ");
    var type = (Console.ReadLine() ?? "").Trim();
    
    Console.Write("Preço (ex: 199,99): ");
    if (!decimal.TryParse(Console.ReadLine(), out var price))
    {
        Console.WriteLine("Preço inválido.");
        return;
    }

    if (string.IsNullOrWhiteSpace(sku) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
    {
        Console.WriteLine("SKU, Nome e Tipo são obrigatórios.");
        return;
    }

    // Usa o DbContext da forma como você fez no Escola
    using var db = new AppDbContext();
    var exists = await db.HardwareComponents.AnyAsync(c => c.Sku == sku);
    if (exists) { Console.WriteLine("Já existe um componente com esse SKU."); return; }

    var component = new HardwareComponent 
    { 
        Sku = sku, 
        Name = name, 
        ComponentType = type,
        Price = price,
        AddedAt = DateTime.UtcNow 
    };
    
    db.HardwareComponents.Add(component);
    await db.SaveChangesAsync();
    Console.WriteLine($"Cadastrado com sucesso! Id: {component.Id}");
}

async Task ListHardwareAsync()
{
    using var db = new AppDbContext();
    var components = await db.HardwareComponents.OrderBy(c => c.Name).ToListAsync();

    if (components.Count == 0) { Console.WriteLine("Nenhum componente encontrado."); return; }

    Console.WriteLine("Id | SKU          | Nome                 | Tipo       | Preço      | Adicionado (UTC)");
    Console.WriteLine(new string('-', 90)); // Linha divisória
    foreach (var c in components)
        Console.WriteLine($"{c.Id,2} | {c.Sku,-12} | {c.Name,-20} | {c.ComponentType,-10} | {c.Price,10:C2} | {c.AddedAt:yyyy-MM-dd HH:mm}");
}

async Task UpdateHardwareAsync()
{
    Console.Write("Informe o Id do componente a atualizar: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

    using var db = new AppDbContext();
    var component = await db.HardwareComponents.FirstOrDefaultAsync(c => c.Id == id);
    if (component is null) { Console.WriteLine("Componente não encontrado."); return; }

    Console.WriteLine($"Atualizando Id {component.Id}. Deixe em branco para manter.");

    Console.WriteLine($"Nome atual : {component.Name}");
    Console.Write("Novo nome  : ");
    var newName = (Console.ReadLine() ?? "").Trim();

    Console.WriteLine($"Preço atual: {component.Price:C2}");
    Console.Write("Novo preço : ");
    var newPriceInput = (Console.ReadLine() ?? "").Trim();

    if (!string.IsNullOrWhiteSpace(newName)) component.Name = newName;
    
    if (!string.IsNullOrWhiteSpace(newPriceInput))
    {
        if (decimal.TryParse(newPriceInput, out var newPrice))
        {
            component.Price = newPrice;
        }
        else
        {
            Console.WriteLine("Formato de preço inválido. Preço não alterado.");
        }
    }

    // (Poderíamos adicionar SKU, Tipo, etc., mas isso aqui já é o suficiente)
    
    await db.SaveChangesAsync();
    Console.WriteLine("Componente atualizado com sucesso.");
}

async Task DeleteHardwareAsync()
{
    Console.Write("Informe o Id do componente a remover: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) { Console.WriteLine("Id inválido."); return; }

    using var db = new AppDbContext();
    var component = await db.HardwareComponents.FirstOrDefaultAsync(c => c.Id == id);
    if (component is null) { Console.WriteLine("Componente não encontrado."); return; }

    // (Opcional: Confirmação)
    Console.Write($"Tem certeza que quer remover '{component.Name}' (SKU: {component.Sku})? (s/n): ");
    if (Console.ReadLine()?.ToLower() != "s")
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }

    db.HardwareComponents.Remove(component);
    await db.SaveChangesAsync();
    Console.WriteLine("Componente removido com sucesso.");
}