using ApiCatalago.Context;
using ApiCatalago.DTO;
using ApiCatalago.DTO.Mappings;
using ApiCatalago.Filters;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using ApiCatalago.Repositories;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using X.PagedList;

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

        private ActionResult<IEnumerable<CategoriaDTO>> getMetaCategOK(IPagedList<Categoria> categorias)
        {
            var meta = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };
            Response.Headers.Append("F-PaginationCategorias", JsonConvert.SerializeObject(meta));
            return Ok(categorias.toListCategoriaDTOs());
        }

        [HttpGet]
        [ServiceFilter(typeof(APILoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _UoW.CategoriaRepository.GetAllAsync();
            return Ok(CategoriaDTOMappingExtentions.toListCategoriaDTOs(categorias));

        }

        [HttpGet("{id:int}", Name = "GetCategoriaId")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {

            var categoria = await _UoW.CategoriaRepository.GetAsync(c => c.Id == id);
            if (categoria is null)
            {
                _logger.LogWarning("Categoria com Id inválido");
                return NotFound($"id: {id} inválido");
            }
            
            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(categoria));

        }

        [HttpGet("PaginationCategoria")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _UoW.CategoriaRepository.GetPagedCategoriasAsync(categoriaParameters);
            return getMetaCategOK(categorias);
        }

        

        [HttpGet("Pagetion/Categoria/filter/Nome")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaFiltroNome categoriaParameters)
        {
            var categorias = await _UoW.CategoriaRepository.GetCatFiltroNomeAsync(categoriaParameters);
            return getMetaCategOK(categorias);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO catDTO)
        {

            if (catDTO is null)
            {
                _logger.LogWarning("Categoria inválida");
                return BadRequest();
            }

            var createdCategory = _UoW.CategoriaRepository.Create(catDTO.toCategoria());
            await _UoW.CommitAsync();

            return new CreatedAtRouteResult("GetCategoriaId", new { id = createdCategory.Id }, CategoriaDTOMappingExtentions.toCategoriaDTO(createdCategory));

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO catDTO)
        {

            if (id != catDTO.Id)
            {
                _logger.LogWarning("Id inválido");
                return BadRequest();
            }
            var catUpdate = _UoW.CategoriaRepository.Update(catDTO.toCategoria());
            await _UoW.CommitAsync();

            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(catUpdate));

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {

            var cat = await _UoW.CategoriaRepository.GetAsync(c => c.Id == id);

            if (cat is null)
            {

                _logger.LogWarning("Categoria não encontrada");
                return NotFound();
            }
            
            var categDel = _UoW.CategoriaRepository.Delete(cat);
            
            await _UoW.CommitAsync();

            return Ok(CategoriaDTOMappingExtentions.toCategoriaDTO(categDel));


        }
    }
}
