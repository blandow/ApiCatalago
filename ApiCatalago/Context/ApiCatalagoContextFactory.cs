using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApiCatalago.Context
{
    public class ApiCatalagoContextFactory : IDesignTimeDbContextFactory<ApiCatalagoContext>
    {
        public ApiCatalagoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApiCatalagoContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=CatalogoDB;Uid=root;Pwd=Abc/123",
                ServerVersion.AutoDetect("Server=localhost;Database=CatalogoDB;Uid=root;Pwd=Abc/123")
            );

            return new ApiCatalagoContext(optionsBuilder.Options);
        }
    }
}
