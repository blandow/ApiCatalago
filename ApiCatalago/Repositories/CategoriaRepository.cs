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
