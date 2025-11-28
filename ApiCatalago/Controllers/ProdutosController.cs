using Microsoft.AspNetCore.Mvc;
using ApiCatalago.Context;

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
    }
}
