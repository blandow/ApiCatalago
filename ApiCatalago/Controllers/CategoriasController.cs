using ApiCatalago.Context;
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
        public ActionResult<IEnumerable<Categoria>> Get()
        {


            try
            {
                return _context.Categorias.AsNoTracking().ToList();

            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);

            }

        }

        [HttpGet("GetAllAsync")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            try
            {
                if (_context.Categorias is null)
                    return NotFound("Categorias não encontradas");

                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpGet("CategoriasProdutos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            try
            {
                return _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToList();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpGet("CategoriasProdutosAsync")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaProdutosAsync()
        {
            try
            {
                if (_context.Categorias is null)
                    return NotFound("Categorias não encontradas");

                return await _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToListAsync();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpGet("{id:int}", Name = "GetCategoriaId")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.Id == id);
                if (categoria is null)
                    return NotFound($"id: {id} inválido");
                return categoria;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpGet("GetCategoriaIdAsync/{id:int:min(0)}", Name = "GetCategoriaIdAsync")]
        public async Task<ActionResult<Categoria>> GetAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

                if (categoria is null)
                    return NotFound($"id: {id} inválido");
                return categoria;
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria cat)
        {
            try
            {
                if (cat is null)
                    return BadRequest();

                _context.Categorias.Add(cat);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetCategoriaId", new { id = cat.Id }, cat);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria cat)
        {
            try
            {
                if (id != cat.Id)
                    return BadRequest();

                _context.Entry(cat).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var cat = _context.Categorias.FirstOrDefault(c => c.Id == id);

                if (cat is null)
                    return NotFound();

                _context.Remove(cat);
                _context.SaveChanges();

                return Ok(cat);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro inesperado: " + e.Message);
            }

        }
    }
}
