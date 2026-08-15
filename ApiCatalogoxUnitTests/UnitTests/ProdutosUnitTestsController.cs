
using ApiCatalago.Context;
using ApiCatalago.DTO.Mappings;
using ApiCatalago.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace ApiCatalogoxUnitTests.UnitTests
{
    internal class ProdutosUnitTestsController
    {
        public IUnitOfWork repository;
        public IMapper mapper;
        public static DbContextOptions<ApiCatalagoContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;Database=CatalogoDB;Uid=root;Pwd=Abc/123";

        static ProdutosUnitTestsController()
        {
            dbContextOptions = new DbContextOptionsBuilder<ApiCatalagoContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
        }
        public ProdutosUnitTestsController()
        {
            var config = new MapperConfiguration(c => 
            { 
                c.AddProfile(new ProdutoDTOMappingProfile()); 
            }, NullLoggerFactory.Instance);

            mapper = config.CreateMapper();

            var context = new ApiCatalagoContext(dbContextOptions);
            repository = new UnitOfWork(context);
        }
    }
}
