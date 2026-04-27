using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories
{
    public class ProdutosRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutosRepository(ApiCatalagoContext context) : base(context)
        {
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutoFiltroPreco produtosParamiters)
        {
            var produtos = GetAll().AsQueryable();
            if(produtosParamiters.Preco.HasValue && !string.IsNullOrEmpty(produtosParamiters.PrecoCriterio))
            {
                switch (produtosParamiters.PrecoCriterio.Trim().ToLower()) 
                {
                    case "maior":
                        produtos = produtos.Where(p => p.Preco > produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                    case "menor":
                        produtos = produtos.Where( p => p.Preco < produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                    case "igual":
                        produtos = produtos.Where(p => p.Preco == produtosParamiters.Preco.Value).OrderBy(p => p.Preco);
                        break;
                }

            }
            return PagedList<Produto>.ToPagedList(produtos, produtosParamiters.PageNumber,produtosParamiters.PageSize);
        }

        public PagedList<Produto> GetProdutosFromParam(ProdutosParamiters produtosParamiters)
        {
            //return GetAll().OrderBy(on => on.Nome).Skip((produtosParamiters.PageNumber - 1) * produtosParamiters.PageSize).Take(produtosParamiters.PageSize).ToList();

            return PagedList<Produto>.ToPagedList
                (
                    GetAll()
                    .OrderBy(p => p.Id)
                    .AsQueryable(),
                    produtosParamiters.PageNumber,
                    produtosParamiters.PageSize
                );
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
