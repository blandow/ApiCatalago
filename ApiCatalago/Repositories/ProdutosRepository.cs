using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories
{
    public class ProdutosRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutosRepository(ApiCatalagoContext context) : base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtosParamiters)
        {
            var produtos = await GetAllAsync();
            var produtosAsQueryable = produtos.AsQueryable();

            if (produtosParamiters.Preco.HasValue && !string.IsNullOrEmpty(produtosParamiters.PrecoCriterio))
            {
                switch (produtosParamiters.PrecoCriterio.Trim().ToLower()) 
                {
                    case "maior":
                        produtosAsQueryable = produtosAsQueryable.Where(p => p.Preco > produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                    case "menor":
                        produtosAsQueryable = produtosAsQueryable.Where( p => p.Preco < produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                    case "igual":
                        produtosAsQueryable = produtosAsQueryable.Where(p => p.Preco == produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                }

            }
            return await produtosAsQueryable.ToPagedListAsync(produtosParamiters.PageNumber,produtosParamiters.PageSize);
        }

        public async Task<IPagedList<Produto>> GetProdutosFromParamAsync(ProdutosParamiters produtosParamiters)
        {
            var produtos = await GetAllAsync();
            var produtosAsQueryable = produtos.OrderBy(p => p.Id).AsQueryable();

            return await produtosAsQueryable.ToPagedListAsync(produtosParamiters.PageNumber, produtosParamiters.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();
            return produtos.Where(c => c.CategoriaId == id);
        }
    }
}
