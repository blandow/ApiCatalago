using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
       Task<IPagedList<Categoria>> GetPagedCategoriasAsync (CategoriaParameters categoriaParameters);
       
       Task<IPagedList<Categoria>> GetCatFiltroNomeAsync (CategoriaFiltroNome categoriaParametros);


    }
}
