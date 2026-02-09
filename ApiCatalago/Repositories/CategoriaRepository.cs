using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApiCatalagoContext _context;

        public CategoriaRepository(ApiCatalagoContext context)
        {
            _context = context;
        }
        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;

        }
        public Categoria Update(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria == null)
                throw new KeyNotFoundException($"Categoria não encontrada id: {id}");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }



    }
}
