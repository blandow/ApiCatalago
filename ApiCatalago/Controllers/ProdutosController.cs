using Microsoft.AspNetCore.Mvc;
using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;


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

        [HttpGet("{id:int}", Name = "GetProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound("Produto não existe");
            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();
            
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetProduto", new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null)
                return NotFound("Id não encontrado");
            
            _context.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    
    }
}
