# LANCHONETE-SENAI

API da lanchonete desenvolvida com **ASP.NET Core Minimal API** e **.NET 8**.

## Pré-requisitos

- SDK do .NET 8
- Git

## Executar

```bash
dotnet restore
dotnet run
```

Por padrão, a API estará disponível em `http://localhost:5000` ou na porta informada pelo console.

## Endpoints

### Listar produtos

```http
GET /api/produtos
```

### Registrar venda

```http
POST /api/vendas
Content-Type: application/json

{
  "cliente": "Maria",
  "pagamento": "PIX",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}
```

A venda somente atualiza o estoque depois que todos os itens forem validados.
