using Microsoft.EntityFrameworkCore;
using ApiCatalago.Models;

namespace ApiCatalago.Context
{
    public class ApiCatalagoContext : DbContext
    {
        public ApiCatalagoContext(DbContextOptions<ApiCatalagoContext> options): base(options)
        {
        }
        DbSet<Categoria>? Categorias { get; set; }
        DbSet<Produto>? Produtos { get; set; }
    
    }
}
