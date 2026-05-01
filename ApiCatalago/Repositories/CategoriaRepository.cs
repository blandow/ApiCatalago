using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using X.PagedList;

namespace ApiCatalago.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApiCatalagoContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCatFiltroNomeAsync(CategoriaFiltroNome categoriaParametros)
        {
            var categorias = await GetAllAsync();


            if(!string.IsNullOrEmpty(categoriaParametros.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaParametros.Nome)).OrderBy(c => c.Id);
            }

            var categFiltradas = await categorias.ToPagedListAsync(categoriaParametros.PageNumber, categoriaParametros.PageSize);

            

            return categFiltradas;
        }

        public async Task<IPagedList<Categoria>> GetPagedCategoriasAsync(CategoriaParameters categoriaParameters)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenada = categorias.OrderBy(c => c.Id);

            return await categoriasOrdenada.ToPagedListAsync(categoriaParameters.PageNumber, categoriaParameters.PageSize);

        }
    }
}
