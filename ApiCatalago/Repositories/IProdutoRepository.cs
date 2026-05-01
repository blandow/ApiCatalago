using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        Task<IPagedList<Produto>> GetProdutosFromParamAsync(ProdutosParamiters produtosParamiters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutoFiltroPreco produtosParamiters);
    }
}
