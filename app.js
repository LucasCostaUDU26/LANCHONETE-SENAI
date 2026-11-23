const listaProdutos = document.querySelector("#lista-produtos");

async function carregarProdutos() {
  try {
    const resposta = await fetch("/api/produtos");

    if (!resposta.ok)
      throw new Error("Não foi possível carregar os produtos.");

    const produtos = await resposta.json();

    listaProdutos.innerHTML = produtos.map(produto => `
      <article class="card">
        <h3>${produto.nome}</h3>
        <p class="preco">R$ ${produto.preco.toFixed(2).replace(".", ",")}</p>
        <p>Estoque: ${produto.estoque}</p>
        <button type="button" data-produto-id="${produto.id}">
          Comprar
        </button>
      </article>
    `).join("");
  } catch (erro) {
    listaProdutos.innerHTML = `<p class="erro">${erro.message}</p>`;
  }
}

carregarProdutos();
