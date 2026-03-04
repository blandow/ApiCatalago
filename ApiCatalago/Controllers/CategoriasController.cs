using ApiCatalago.Context;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using ApiCatalago.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiCatalago.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(APILoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return Ok(_repository.GetCategorias());

        }

        //[HttpGet("GetAllAsync")]
        //public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        //{

        //    if (_context.Categorias is null)
        //        return NotFound("Categorias não encontradas");

        //    return await _context.Categorias.AsNoTracking().ToListAsync();

        //}

        //[HttpGet("CategoriasProdutos")]
        //public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        //{

        //    return _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToList();

        //}

        //[HttpGet("CategoriasProdutosAsync")]
        //public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaProdutosAsync()
        //{

        //    if (_context.Categorias is null)
        //        return NotFound("Categorias não encontradas");

        //    return await _context.Categorias.Include(c => c.Produtos).Where(c => c.Id <= 10).ToListAsync();

        //}

        [HttpGet("{id:int}", Name = "GetCategoriaId")]
        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _repository.GetCategoria(id);
            if (categoria is null)
            {

                _logger.LogWarning("Categoria com Id inválido");
                return NotFound($"id: {id} inválido");
            }

            return Ok(categoria);

        }

        //[HttpGet("GetCategoriaIdAsync/{id:int:min(0)}", Name = "GetCategoriaIdAsync")]
        //public async Task<ActionResult<Categoria>> GetAsync(int id)
        //{

        //    var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        //    if (categoria is null)
        //        return NotFound($"id: {id} inválido");
        //    return categoria;

        //}

        [HttpPost]
        public ActionResult Post(Categoria cat)
        {

            if (cat is null)
            {
                _logger.LogWarning("Categoria inválida");
                return BadRequest();
            }

            var createdCategory = _repository.Create(cat);

            return new CreatedAtRouteResult("GetCategoriaId", new { id = createdCategory.Id }, createdCategory);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria cat)
        {

            if (id != cat.Id)
            {
                _logger.LogWarning("Id inválido");
                return BadRequest();
            }

            return Ok(_repository.Update(cat));

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var cat = _repository.GetCategoria(id);

            if (cat is null)
            {

                _logger.LogWarning("Categoria não encontrada");
                return NotFound();
            }

            return Ok(_repository.Delete(id));


        }
    }
}
