using ApiCatalago.Context;
using ApiCatalago.DTO;
using ApiCatalago.DTO.Mappings;
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
        private readonly IUnitOfWork _UoW;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork UoW)
        {
            
            _logger = logger;
            _UoW = UoW;
        }

        [HttpGet]
        [ServiceFilter(typeof(APILoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            return Ok(CategoriaDTOMappingExtentions.toListCategoriaDTOs(_UoW.CategoriaRepository.GetAll()));

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
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _UoW.CategoriaRepository.Get(c => c.Id == id);
            if (categoria is null)
            {

                _logger.LogWarning("Categoria com Id inválido");
                return NotFound($"id: {id} inválido");
            }
            
            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(categoria));

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
        public ActionResult<CategoriaDTO> Post(CategoriaDTO catDTO)
        {

            if (catDTO is null)
            {
                _logger.LogWarning("Categoria inválida");
                return BadRequest();
            }

            var createdCategory = _UoW.CategoriaRepository.Create(catDTO.toCategoria());
            _UoW.Commit();

            return new CreatedAtRouteResult("GetCategoriaId", new { id = createdCategory.Id }, CategoriaDTOMappingExtentions.toCategoriaDTO(createdCategory));

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoriaDTO catDTO)
        {

            if (id != catDTO.Id)
            {
                _logger.LogWarning("Id inválido");
                return BadRequest();
            }
            var catUpdate = _UoW.CategoriaRepository.Update(catDTO.toCategoria());
            _UoW.Commit();

            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(catUpdate));

        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {

            var cat = _UoW.CategoriaRepository.Get(c => c.Id == id);

            if (cat is null)
            {

                _logger.LogWarning("Categoria não encontrada");
                return NotFound();
            }
            
            var categDel = _UoW.CategoriaRepository.Delete(cat);
            _UoW.Commit();
            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(categDel));


        }
    }
}
