namespace ApiCatalago.Repositories
{
    public interface IUnitOfWork
    {
        public IProdutoRepository ProdutoRepository{ get; }
        public ICategoriaRepository CategoriaRepository{ get; }
        public Task CommitAsync();
    }
}
