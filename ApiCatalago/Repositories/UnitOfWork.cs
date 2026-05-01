using ApiCatalago.Context;

namespace ApiCatalago.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {


        private IProdutoRepository? _produtoRep;

        private ICategoriaRepository? _categoriaRep;

        public ApiCatalagoContext _context;

        public UnitOfWork(ApiCatalagoContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository {
            get {
                return _produtoRep = _produtoRep ?? new ProdutosRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository { 
            get {
                return _categoriaRep = _categoriaRep ?? new CategoriaRepository(_context);
            }
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose() 
        {
            _context.Dispose();
        }

    }
}
