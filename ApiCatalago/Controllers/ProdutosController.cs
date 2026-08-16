using ApiCatalago.DTO;
using ApiCatalago.Models;
using ApiCatalago.Pagination;
using ApiCatalago.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace ApiCatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProdutosController : ControllerBase
    {

        private readonly ILogger<ProdutosController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _UoW;
        public ProdutosController(ILogger<ProdutosController> logger, IUnitOfWork uoW, IMapper mapper)
        {

            _logger = logger;
            _UoW = uoW;
            _mapper = mapper;
        }

        private ActionResult<IEnumerable<ProdutoDTO>> getProdutoMeta(IPagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.Count,
                produtos.PageSize,
                produtos.PageNumber,
                produtos.TotalItemCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage


            };
            Response.Headers.Append("F-PaginationProduct", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }

        [HttpGet ("produtos/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosCategoria(int id)
        {
            var produtos = await _UoW.ProdutoRepository.GetProdutosPorCategoriaAsync(id);
            if(produtos is null)
            {
                _logger.LogError("categoria de produtos não encontrados");
                return NotFound();
            }
            

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }

        [HttpGet]
        [Authorize("userOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = await _UoW.ProdutoRepository.GetAllAsync();
         
            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }

        [HttpGet("{id:int}", Name = "GetProdutoId")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produtos = await _UoW.ProdutoRepository.GetAsync(p => p.Id == id);
            return Ok(_mapper.Map<ProdutoDTO>(produtos));

        }

        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParamiters produtosParamiters) 
        {
            var produtos = await _UoW.ProdutoRepository.GetProdutosFromParamAsync(produtosParamiters);
            return getProdutoMeta(produtos);
        }

        [HttpGet("Pagination/Produtos/Preco")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPreco([FromQuery] ProdutoFiltroPreco produtosParamiters)
        {
            if (!produtosParamiters.PrecoCriterio.Trim().Equals("maior", StringComparison.OrdinalIgnoreCase)
                && !produtosParamiters.PrecoCriterio.Trim().Equals("menor", StringComparison.OrdinalIgnoreCase) 
                && !produtosParamiters.PrecoCriterio.Trim().Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("Criterio de preço inválido");
                return BadRequest("Criterio de preço inválido");
            }

            var produtos = await _UoW.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosParamiters);
            return getProdutoMeta(produtos);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "superAdminOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
        {

            if (produtoDTO is null)
            {

                _logger.LogError("objeto inválido");
                return BadRequest();
            }

            var prodNew = _UoW.ProdutoRepository.Create(_mapper.Map<Produto>(produtoDTO));
            await _UoW.CommitAsync();
            var prodnewDTO = _mapper.Map<ProdutoDTO>(prodNew);

            return new CreatedAtRouteResult("GetProduto", new { id = prodnewDTO.Id }, prodnewDTO);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produtoDTO)
        {
            var produtos = await _UoW.ProdutoRepository.GetAsync(p => p.Id == produtoDTO.Id);

            if (id != produtoDTO.Id || produtos is null)
            {
                _logger.LogError($"ID: {id} DIFERENTE DO PRODUTO");
                return BadRequest();
            }
            var prodUpdate = _UoW.ProdutoRepository.Update(_mapper.Map<Produto>(produtoDTO));
            await _UoW.CommitAsync();

            return Ok(_mapper.Map<ProdutoDTO>(prodUpdate));

        }

        [HttpPatch("{id}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> produtoDTO)
        {
            if(produtoDTO is null|| id <= 0)
            {
                _logger.LogError($"ID menor ou igual a zero ou produto vazio");
                return BadRequest();
            }

            var produto = await _UoW.ProdutoRepository.GetAsync(p => p.Id == id);
            if(produto is null)
            {
                _logger.LogError($"produto não encontrado");
                return BadRequest();
            }

            var produtoReqDTO = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
            produtoDTO.ApplyTo(produtoReqDTO, ModelState);

            if(!ModelState.IsValid || !TryValidateModel(produtoReqDTO))
            {
                _logger.LogError("Erro no modelo de estados");
                return BadRequest(ModelState);
            }
            _mapper.Map(produtoReqDTO, produto);
            _UoW.ProdutoRepository.Update(produto);
            await _UoW.CommitAsync();
            

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "exclusivePolicyOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {

            var produto = await _UoW.ProdutoRepository.GetAsync(p => p.Id == id);
            if ( produto is null)
            {
                _logger.LogError("Produto não encontrado");
                return NotFound("Id não encontrado");
            }
            var deletedProd = _UoW.ProdutoRepository.Delete(produto);
            await _UoW.CommitAsync(); 

            return Ok(_mapper.Map<ProdutoDTO>(deletedProd));

        }

    }
}
