using ApiCatalago.Context;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApiCatalagoContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCatFiltroNome(CategoriaFiltroNome categoriaParametros)
        {
            var categorias = GetAll().AsQueryable();

            if(!string.IsNullOrEmpty(categoriaParametros.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaParametros.Nome)).OrderBy(c => c.Id);
            }

            return PagedList<Categoria>.ToPagedList(categorias, categoriaParametros.PageNumber, categoriaParametros.PageSize);
        }

        public PagedList<Categoria> GetPagedCategorias(CategoriaParameters categoriaParameters)
        {
            return PagedList<Categoria>.ToPagedList 
            (
               GetAll().OrderBy(c => c.Id).AsQueryable(),
               categoriaParameters.PageNumber,
               categoriaParameters.PageSize
            );

        }
    }
}
