using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApiCatalagoContext _context;

        public CategoriasController(ApiCatalagoContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.ToList();
        }

        [HttpGet("CategoriasProdutos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            return _context.Categorias.Include(c => c.Produtos).ToList();
        }

        [HttpGet("{id:int}", Name = "GetCategoriaId")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria is null)
                return NotFound($"id: {id} inválido");
            return categoria;
        }

        [HttpPost]
        public ActionResult Post(Categoria cat)
        {
            if (cat is null)
                return BadRequest();

            _context.Categorias.Add(cat);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetCategoriaId", new { id = cat.Id }, cat);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria cat)
        {
            if (id != cat.Id)
                return BadRequest();
            
            _context.Entry(cat).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var cat = _context.Categorias.FirstOrDefault(c => c.Id == id);
            
            if (cat is null)
                return NotFound();

            _context.Remove(cat);
            _context.SaveChanges();

            return Ok(cat);
        }
    }
}
