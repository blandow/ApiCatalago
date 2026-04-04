using ApiCatalago.Models;
using ApiCatalago.Pagination;

namespace ApiCatalago.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
       PagedList<Categoria> GetPagedCategorias (CategoriaParameters categoriaParameters);

    }
}
