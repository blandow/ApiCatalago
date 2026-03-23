using Microsoft.AspNetCore.Mvc;
using ApiCatalago.Context;
using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using ApiCatalago.Repositories;


namespace ApiCatalago.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {

        private readonly ILogger<ProdutosController> _logger;

        private readonly IUnitOfWork _UoW;
        public ProdutosController(ILogger<ProdutosController> logger,  IUnitOfWork uoW)
        {

            _logger = logger;
            _UoW = uoW;
        }
        [HttpGet ("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _UoW.ProdutoRepository.GetProdutosPorCategoria(id);
            if(produtos is null)
            {
                _logger.LogError("categoria de produtos não encontrados");
                return NotFound();
            }

            return Ok(produtos);
        }
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {

            return Ok(_UoW.ProdutoRepository.GetAll());

        }

        //[HttpGet("GetAllProductAsync")]
        //public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        //{

        //        var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
        //        if (produtos is null)
        //            return NotFound("Produtos não encontrados");
        //        return produtos;

        //}

        [HttpGet("{id:int}", Name = "GetProduto")]
        public ActionResult<Produto> Get(int id)
        {

            return Ok(_UoW.ProdutoRepository.Get(p => p.Id == id));

        }

        //[HttpGet("GetProdutoAsync/{id:int:min(1)}", Name = "GetProdutoAsync")]
        //public async Task<ActionResult<Produto>> GetAsync(int id)
        //{
        //        var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        //        if (produto is null)
        //            return NotFound("Produto não existe");
        //        return produto;

        //}

        [HttpPost]
        public ActionResult Post(Produto produto)
        {

            if (produto is null)
            {

                _logger.LogError("objeto inválido");
                return BadRequest();
            }

            var prodNew = _UoW.ProdutoRepository.Create(produto);
            _UoW.Commit();

            return new CreatedAtRouteResult("GetProduto", new { id = prodNew.Id }, prodNew);

        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {

            if (id != produto.Id)
            {
                _logger.LogError($"ID: {id} DIFERENTE DO PRODUTO");
                return BadRequest();
            }
            var prodUpdate = _UoW.ProdutoRepository.Update(produto);
            _UoW.Commit();

            return Ok(prodUpdate);

        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var produto = _UoW.ProdutoRepository.Get(p => p.Id == id);
            if ( produto is null)
            {
                _logger.LogError("Produto não encontrado");
                return NotFound("Id não encontrado");
            }
            var deletedProd = _UoW.ProdutoRepository.Delete(produto);
            _UoW.Commit(); 

            return Ok(deletedProd);

        }

    }
}
