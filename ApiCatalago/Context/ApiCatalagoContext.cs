using Microsoft.EntityFrameworkCore;
using ApiCatalago.Models;

namespace ApiCatalago.Context
{
    public class ApiCatalagoContext : DbContext
    {
        public ApiCatalagoContext(DbContextOptions<ApiCatalagoContext> options): base(options)
        {
        }
        public DbSet<Categoria> ? Categorias { get; set; }
        public DbSet<Produto> ? Produtos { get; set; }
    
    }
}
