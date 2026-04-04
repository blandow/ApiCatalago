using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
        //IEnumerable<Produto> GetProdutosFromParam(ProdutosParamiters produtosParamiters);
        PagedList<Produto> GetProdutosFromParam(ProdutosParamiters produtosParamiters);
    }
}
