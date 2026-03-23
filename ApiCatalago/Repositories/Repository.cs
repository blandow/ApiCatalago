using ApiCatalago.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiCatalago.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApiCatalagoContext _context;
        public Repository(ApiCatalagoContext context)
        {
            _context = context;
        }
        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
           // _context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //_context.SaveChanges();
            return entity;
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public T Update(T entity)
        {
            //_context.Set<T>().Update(entity); //alternativa de update
            _context.Entry(entity).State = EntityState.Modified;
            //_context.SaveChanges();
            return entity;
        }
    }
}
