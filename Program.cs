var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var produtos = new List<Produto>
{
    new Produto(1, "X-Burger Clássico", 22.90m, 30)
};

app.MapGet("/api/produtos", () => Results.Ok(produtos));

app.MapPost("/api/vendas", (VendaDto venda) =>
{
    if (venda.Itens is null || venda.Itens.Count == 0)
        return Results.BadRequest(new { Erro = "Nenhum item informado." });

    if (venda.Itens.Any(item => item.Quantidade <= 0))
        return Results.BadRequest(new { Erro = "Quantidade inválida." });

    var itensAgrupados = venda.Itens
        .GroupBy(item => item.ProdutoId)
        .Select(grupo => new
        {
            ProdutoId = grupo.Key,
            Quantidade = grupo.Sum(item => item.Quantidade)
        })
        .ToList();

    decimal total = 0;

    foreach (var item in itensAgrupados)
    {
        var produto = produtos.FirstOrDefault(p => p.Id == item.ProdutoId);

        if (produto is null)
            return Results.BadRequest(new { Erro = $"Produto {item.ProdutoId} não encontrado." });

        if (produto.Estoque < item.Quantidade)
            return Results.BadRequest(new { Erro = "Estoque insuficiente.", Produto = produto.Nome });

        total += produto.Preco * item.Quantidade;
    }

    foreach (var item in itensAgrupados)
    {
        var produto = produtos.First(p => p.Id == item.ProdutoId);
        produto.Estoque -= item.Quantidade;
    }

    return Results.Created("/api/vendas", new
    {
        Cliente = venda.Cliente,
        Pagamento = venda.Pagamento,
        Total = total
    });
});

app.Run();

record Produto(int Id, string Nome, decimal Preco, int Estoque)
{
    public int Estoque { get; set; } = Estoque;
}

record ItemVendaDto(int ProdutoId, int Quantidade);
record VendaDto(string Cliente, string Pagamento, List<ItemVendaDto> Itens);
