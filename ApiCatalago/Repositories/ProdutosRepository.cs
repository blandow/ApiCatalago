using ApiCatalago.Context;
using ApiCatalago.Models;

namespace ApiCatalago.Repositories
{
    public class ProdutosRepository : IProdutoRepository
    {
        private readonly ApiCatalagoContext _context;

        public ProdutosRepository(ApiCatalagoContext context)
        {
            _context = context;
        }

        public Produto Create(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(produto.Id + " - ID vazio");
            }
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto Delete(int id)
        {
            var prodAux = _context.Produtos.Find(id);
            if (prodAux == null)
            {
                throw new KeyNotFoundException(id + " - ID não encontrado");
            }
            _context.Produtos.Remove(prodAux);
            _context.SaveChanges();
            return prodAux;
        }

        public Produto GetProduto(int id)
        {
            return _context.Produtos.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Produto> GetProdutos()
        {
            return _context.Produtos.ToList();
        }

        public Produto Update(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException("Favor inserir valor válido");
            }
            _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return produto;
        }
    }
}
