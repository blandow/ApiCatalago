using Microsoft.EntityFrameworkCore;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApiCatalago.Context
{
    public class ApiCatalagoContext : IdentityDbContext<ApplicationUser>
    {
        public ApiCatalagoContext(DbContextOptions<ApiCatalagoContext> options): base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Categoria> ? Categorias { get; set; }
        public DbSet<Produto> ? Produtos { get; set; }
    
    }
}
