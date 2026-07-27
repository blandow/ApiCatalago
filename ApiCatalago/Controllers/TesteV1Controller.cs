using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalago.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion() 
        {
            return "Teste V1 - Get - Hellow World";
        }
    }
}
