using ApiCatalago.Models;

namespace ApiCatalago.Repositories
{
    public interface IProdutoRepository
    {
        public IEnumerable<Produto> GetProdutos();
        public Produto GetProduto(int id);
        public Produto Create(Produto produto);
        public Produto Update(Produto produto);
        public Produto Delete(int id);
    }
}
