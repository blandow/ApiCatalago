using Microsoft.AspNetCore.Mvc;
using ApiCatalago.Models;
using ApiCatalago.Repositories;
using AutoMapper;
using ApiCatalago.DTO;
using Microsoft.AspNetCore.JsonPatch;
using ApiCatalago.Pagination;
using Newtonsoft.Json;
using MathNet.Numerics;


namespace ApiCatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        private ActionResult<IEnumerable<ProdutoDTO>> getProdutoMeta(PagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };
            Response.Headers.Append("F-PaginationProduct", JsonConvert.SerializeObject(metadata));

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }

        [HttpGet ("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            var produtos = _UoW.ProdutoRepository.GetProdutosPorCategoria(id);
            if(produtos is null)
            {
                _logger.LogError("categoria de produtos não encontrados");
                return NotFound();
            }
            

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(produtos));
        }
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {

            return Ok(_mapper.Map<IEnumerable<ProdutoDTO>>(_UoW.ProdutoRepository.GetAll()));

        }

        //[HttpGet("GetAllProductAsync")]
        //public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        //{

        //        var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
        //        if (produtos is null)
        //            return NotFound("Produtos não encontrados");
        //        return produtos;

        //}

        [HttpGet("{id:int}", Name = "GetProdutoId")]
        public ActionResult<ProdutoDTO> Get(int id)
        {

            return Ok(_mapper.Map<ProdutoDTO>(_UoW.ProdutoRepository.Get(p => p.Id == id)));

        }

        //[HttpGet("GetProdutoAsync/{id:int:min(1)}", Name = "GetProdutoAsync")]
        //public async Task<ActionResult<Produto>> GetAsync(int id)
        //{
        //        var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        //        if (produto is null)
        //            return NotFound("Produto não existe");
        //        return produto;

        //}

        [HttpGet("Pagination")]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParamiters produtosParamiters) 
        {
            var produtos = _UoW.ProdutoRepository.GetProdutosFromParam(produtosParamiters);
            return getProdutoMeta(produtos);
        }
        [HttpGet("Pagination/Produtos/Preco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPreco([FromQuery] ProdutoFiltroPreco produtosParamiters)
        {
            if(produtosParamiters.PrecoCriterio.Trim().ToLower() != "maior" && produtosParamiters.PrecoCriterio.Trim().ToLower() != "menor" && produtosParamiters.PrecoCriterio.Trim().ToLower() != "igual")
            {
                _logger.LogError("Criterio de preço inválido");
                return BadRequest("Criterio de preço inválido");
            }

            var produtos = _UoW.ProdutoRepository.GetProdutosFiltroPreco(produtosParamiters);
            return getProdutoMeta(produtos);
        }


        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDTO)
        {

            if (produtoDTO is null)
            {

                _logger.LogError("objeto inválido");
                return BadRequest();
            }

            var prodNew = _UoW.ProdutoRepository.Create(_mapper.Map<Produto>(produtoDTO));
            _UoW.Commit();
            var prodnewDTO = _mapper.Map<ProdutoDTO>(prodNew);

            return new CreatedAtRouteResult("GetProduto", new { id = prodnewDTO.Id }, prodnewDTO);

        }

        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDTO)
        {

            if (id != produtoDTO.Id || _UoW.ProdutoRepository.Get( p => p.Id == produtoDTO.Id) is null)
            {
                _logger.LogError($"ID: {id} DIFERENTE DO PRODUTO");
                return BadRequest();
            }
            var prodUpdate = _UoW.ProdutoRepository.Update(_mapper.Map<Produto>(produtoDTO));
            _UoW.Commit();

            return Ok(_mapper.Map<ProdutoDTO>(prodUpdate));

        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> produtoDTO)
        {
            if(produtoDTO is null|| id <= 0)
            {
                _logger.LogError($"ID menor ou igual a zero ou produto vazio");
                return BadRequest();
            }

            var produto = _UoW.ProdutoRepository.Get(p => p.Id == id);
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
            _UoW.Commit();
            

            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {

            var produto = _UoW.ProdutoRepository.Get(p => p.Id == id);
            if ( produto is null)
            {
                _logger.LogError("Produto não encontrado");
                return NotFound("Id não encontrado");
            }
            var deletedProd = _UoW.ProdutoRepository.Delete(produto);
            _UoW.Commit(); 

            return Ok(_mapper.Map<ProdutoDTO>(deletedProd));

        }

    }
}
