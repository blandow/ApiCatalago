using Microsoft.AspNetCore.Mvc;
using ApiCatalago.Context;
using ApiCatalago.Models;


namespace ApiCatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiCatalagoContext _context;



        public ProdutosController(ApiCatalagoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get() 
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
                return NotFound("Produtos não encontrados");
            return produtos;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound("Produto não existe");
            return produto;
        }

    }
}
