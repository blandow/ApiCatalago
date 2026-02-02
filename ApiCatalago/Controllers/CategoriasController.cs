using ApiCatalago.Context;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        [ServiceFilter(typeof(APILoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.AsNoTracking().ToList();

        }

        [HttpGet("GetAllAsync")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {

            if (_context.Categorias is null)
                return NotFound("Categorias não encontradas");

            return await _context.Categorias.AsNoTracking().ToListAsync();

        }

        [HttpGet("CategoriasProdutos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            return _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToList();

        }

        [HttpGet("CategoriasProdutosAsync")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaProdutosAsync()
        {

            if (_context.Categorias is null)
                return NotFound("Categorias não encontradas");

            return await _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToListAsync();


        }

        [HttpGet("{id:int}", Name = "GetCategoriaId")]
        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (categoria is null)
                return NotFound($"id: {id} inválido");
            return categoria;


        }

        [HttpGet("GetCategoriaIdAsync/{id:int:min(0)}", Name = "GetCategoriaIdAsync")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {

            var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

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
